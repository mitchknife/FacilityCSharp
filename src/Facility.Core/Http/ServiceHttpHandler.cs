using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.RegularExpressions;

namespace Facility.Core.Http;

/// <summary>
/// A service HTTP handler.
/// </summary>
public abstract class ServiceHttpHandler : DelegatingHandler
{
	/// <summary>
	/// Attempts to handle the HTTP request.
	/// </summary>
	public abstract Task<HttpResponseMessage?> TryHandleHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken);

	/// <summary>
	/// Creates an instance.
	/// </summary>
	protected ServiceHttpHandler(ServiceHttpHandlerSettings? settings, ServiceHttpHandlerDefaults defaults)
	{
		settings ??= new ServiceHttpHandlerSettings();

		m_rootPath = (settings.RootPath ?? "").TrimEnd('/');
		m_synchronous = settings.Synchronous;
		m_contentSerializer = settings.ContentSerializer ?? defaults.ContentSerializer ?? HttpContentSerializer.Legacy;
		m_bytesSerializer = settings.BytesSerializer ?? BytesHttpContentSerializer.Instance;
		m_textSerializer = settings.TextSerializer ?? TextHttpContentSerializer.Instance;
		m_aspects = settings.Aspects;
		m_skipRequestValidation = settings.SkipRequestValidation;
		m_skipResponseValidation = settings.SkipResponseValidation;
		m_disableChunkedTransfer = settings.DisableChunkedTransfer;
	}

	/// <summary>
	/// Creates an instance.
	/// </summary>
	[Obsolete("Regenerate code to use the new constructor.")]
	protected ServiceHttpHandler(ServiceHttpHandlerSettings? settings)
		: this(settings, new ServiceHttpHandlerDefaults())
	{
	}

	/// <summary>
	/// Attempts to handle a service method.
	/// </summary>
	protected async Task<HttpResponseMessage?> TryHandleServiceMethodAsync<TRequest, TResponse>(HttpMethodMapping<TRequest, TResponse> mapping, HttpRequestMessage httpRequest, Func<TRequest, CancellationToken, Task<ServiceResult<TResponse>>> invokeMethodAsync, CancellationToken cancellationToken)
		where TRequest : ServiceDto, new()
		where TResponse : ServiceDto, new()
	{
		if (mapping == null)
			throw new ArgumentNullException(nameof(mapping));
		if (httpRequest == null)
			throw new ArgumentNullException(nameof(httpRequest));
		if (invokeMethodAsync == null)
			throw new ArgumentNullException(nameof(invokeMethodAsync));
		if (httpRequest.RequestUri == null)
			throw new ArgumentException("RequestUri must be specified.", nameof(httpRequest));

		if (httpRequest.Method != mapping.HttpMethod)
			return null;

		var pathParameters = TryMatchHttpRoute(httpRequest.RequestUri, m_rootPath + mapping.Path);
		if (pathParameters == null)
			return null;

		var context = new ServiceHttpContext();
		ServiceHttpContext.SetContext(httpRequest, context);

		var aspectHttpResponse = await AdaptTask(RequestReceivedAsync(httpRequest, cancellationToken)).ConfigureAwait(true);
		if (aspectHttpResponse != null)
			return aspectHttpResponse;

		ServiceErrorDto? error = null;

		object? requestBody = null;
		if (mapping.RequestBodyType != null)
		{
			try
			{
				var serializer = GetHttpContentSerializer(mapping.RequestBodyType);
				var requestResult = await AdaptTask(serializer.ReadHttpContentAsync(mapping.RequestBodyType, httpRequest.Content, cancellationToken)).ConfigureAwait(true);
				if (requestResult.IsFailure)
					error = requestResult.Error;
				else
					requestBody = requestResult.Value;
			}
			catch (Exception exception) when (ShouldCreateErrorFromException(exception))
			{
				// cancellation can cause the wrong exception
				cancellationToken.ThrowIfCancellationRequested();

				// error reading request body
				error = CreateErrorFromException(exception);
			}
		}

		TResponse? response = null;
		if (error == null)
		{
			var request = mapping.CreateRequest(requestBody);

			var uriParameters = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
			foreach (var queryParameter in ParseQueryString(httpRequest.RequestUri.Query))
				uriParameters[queryParameter.Key] = queryParameter.Value[0];
			foreach (var pathParameter in pathParameters)
				uriParameters[pathParameter.Key] = pathParameter.Value;
			request = mapping.SetUriParameters(request, uriParameters);
			request = mapping.SetRequestHeaders(request, HttpServiceUtility.CreateDictionaryFromHeaders(httpRequest.Headers, httpRequest.Content?.Headers)!);

			context.Request = request;

			if (!m_skipRequestValidation && !request.Validate(out var requestErrorMessage))
			{
				error = ServiceErrors.CreateInvalidRequest(requestErrorMessage);
			}
			else
			{
				var methodResult = await invokeMethodAsync(request, cancellationToken).ConfigureAwait(true);
				if (methodResult.IsFailure)
				{
					error = methodResult.Error;
				}
				else
				{
					response = methodResult.Value;

					if (!m_skipResponseValidation && !response.Validate(out var responseErrorMessage))
					{
						error = ServiceErrors.CreateInvalidResponse(responseErrorMessage);
						response = null;
					}
				}
			}

			context.Result = error != null ? ServiceResult.Failure(error) : ServiceResult.Success<ServiceDto>(response!);
		}

		HttpResponseMessage httpResponse;
		if (error == null)
		{
			var responseMappingGroups = mapping.ResponseMappings
				.GroupBy(x => x.MatchesResponse(response!))
				.Where(x => x.Key != false)
				.OrderByDescending(x => x.Key)
				.ToList();
			if (responseMappingGroups.Count >= 1 && responseMappingGroups[0].Count() == 1)
			{
				var responseMapping = responseMappingGroups[0].Single();
				httpResponse = new HttpResponseMessage(responseMapping.StatusCode);

				var responseHeaders = mapping.GetResponseHeaders(response!);
				var headersResult = HttpServiceUtility.TryAddNonContentHeaders(httpResponse.Headers, responseHeaders);
				if (headersResult.IsFailure)
					throw new InvalidOperationException(headersResult.Error!.Message);

				if (responseMapping.ResponseBodyType != null)
				{
					var serializer = GetHttpContentSerializer(responseMapping.ResponseBodyType);
					var mediaType = responseMapping.ResponseBodyContentType ?? responseHeaders?.GetContentType() ?? GetAcceptedMediaType(httpRequest, serializer);
					httpResponse.Content = serializer.CreateHttpContent(responseMapping.GetResponseBody(response!)!, mediaType);
					if (m_disableChunkedTransfer)
						await httpResponse.Content.LoadIntoBufferAsync().ConfigureAwait(false);
				}
			}
			else
			{
				throw new InvalidOperationException($"Found {responseMappingGroups.Sum(x => x.Count())} valid HTTP responses for {typeof(TResponse).Name}: {response}");
			}
		}
		else
		{
			var statusCode = error.Code == null ? HttpStatusCode.InternalServerError :
				(TryGetCustomHttpStatusCode(error.Code) ?? HttpServiceErrors.TryGetHttpStatusCode(error.Code) ?? HttpStatusCode.InternalServerError);
			httpResponse = new HttpResponseMessage(statusCode);
			if (statusCode != HttpStatusCode.NoContent && statusCode != HttpStatusCode.NotModified)
			{
				var mediaType = GetAcceptedMediaType(httpRequest, m_contentSerializer);
				httpResponse.Content = m_contentSerializer.CreateHttpContent(error, mediaType);
				if (m_disableChunkedTransfer)
					await httpResponse.Content.LoadIntoBufferAsync().ConfigureAwait(false);
			}
		}

		httpResponse.RequestMessage = httpRequest;
		await AdaptTask(ResponseReadyAsync(httpResponse, cancellationToken)).ConfigureAwait(true);

		return httpResponse;
	}

	/// <summary>
	/// Returns the HTTP status code for a custom error code.
	/// </summary>
	protected virtual HttpStatusCode? TryGetCustomHttpStatusCode(string errorCode) => null;

	/// <summary>
	/// Handle or delegate the HTTP request.
	/// </summary>
	protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		return await TryHandleHttpRequestAsync(request, cancellationToken).ConfigureAwait(true) ??
			await base.SendAsync(request, cancellationToken).ConfigureAwait(true);
	}

	/// <summary>
	/// Called to determine if an error object should be created from an unexpected exception.
	/// </summary>
	protected virtual bool ShouldCreateErrorFromException(Exception exception)
	{
		var exceptionTypeName = exception.GetType().FullName;
		return exceptionTypeName != null && exceptionTypeName.StartsWith("System.Web.", StringComparison.Ordinal);
	}

	/// <summary>
	/// Called to create an error object from an unexpected exception.
	/// </summary>
	protected virtual ServiceErrorDto CreateErrorFromException(Exception exception) =>
		ServiceErrors.CreateInvalidRequest("Unexpected error while reading request body.");

	/// <summary>
	/// Makes a task synchronous if necessary.
	/// </summary>
	[SuppressMessage("Performance", "CA1849:Call async methods when in an async method", Justification = "Task is completed.")]
	protected Task AdaptTask(Task task)
	{
		if (!m_synchronous)
			return task;

#pragma warning disable CA1849
		task.GetAwaiter().GetResult();
#pragma warning restore CA1849
		return Task.CompletedTask;
	}

	/// <summary>
	/// Makes a task synchronous if necessary.
	/// </summary>
	protected Task<T> AdaptTask<T>(Task<T> task)
	{
		if (!m_synchronous)
			return task;

		return Task.FromResult(task.GetAwaiter().GetResult());
	}

	private static IReadOnlyDictionary<string, string>? TryMatchHttpRoute(Uri requestUri, string routePath)
	{
		var requestPath = requestUri.AbsolutePath.Trim('/');
		routePath = routePath.Trim('/');

		if (routePath.IndexOfOrdinal('{') != -1)
		{
			// ReSharper disable once RedundantEnumerableCastCall (needed for .NET Standard 2.0)
			var names = s_regexPathParameterRegex.Matches(routePath).Cast<Match>().Select(x => x.Groups[1].ToString()).ToList();
			var regexPattern = Regex.Escape(routePath);
			foreach (var name in names)
				regexPattern = regexPattern.ReplaceOrdinal("\\{" + name + "}", "(?'" + name + "'[^/]+)");
			regexPattern = "^(?:" + regexPattern + ")$";
			var match = new Regex(regexPattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase).Match(requestPath);
			return match.Success ? names.ToDictionary(name => name, name => Uri.UnescapeDataString(match.Groups[name].ToString())) : null;
		}

		if (string.Equals(requestPath, routePath, StringComparison.OrdinalIgnoreCase))
			return s_emptyDictionary;

		return null;
	}

	private static IReadOnlyDictionary<string, IReadOnlyList<string>> ParseQueryString(string query)
	{
		if (query.Length != 0 && query[0] == '?')
			query = query.Substring(1);

		return query.Split('&')
			.Select(x => x.Split(new[] { '=' }, 2))
			.GroupBy(x => Uri.UnescapeDataString(x[0]), x => Uri.UnescapeDataString(x.Length == 1 ? "" : x[1]), StringComparer.OrdinalIgnoreCase)
			.ToDictionary(x => x.Key, x => (IReadOnlyList<string>) x.ToList());
	}

	private string? GetAcceptedMediaType(HttpRequestMessage httpRequest, HttpContentSerializer serializer)
	{
		var mediaType = httpRequest.Headers.Accept
			.Where(x => x.MediaType is not null)
			.OrderByDescending(x => x.Quality)
			.ThenBy(x => !x.MediaType!.EndsWith("/*", StringComparison.Ordinal) ? 0 : x.MediaType != "*/*" ? 1 : 2)
			.Select(x => x.MediaType)
			.Select(x => x!)
			.FirstOrDefault(x => x == "*/*" || serializer.IsAcceptedMediaType(x));

		// use default media type for universal wildcard
		if (mediaType is null or "*/*")
			return null;

		// use first matching media type for prefix wildcard
		if (mediaType.EndsWith("/*", StringComparison.Ordinal))
		{
			var prefix = mediaType.Substring(0, mediaType.Length - 1);
			return serializer.AcceptMediaTypes.FirstOrDefault(x => x.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
		}

		return mediaType;
	}

	private async Task<HttpResponseMessage?> RequestReceivedAsync(HttpRequestMessage httpRequest, CancellationToken cancellationToken)
	{
		if (m_aspects != null)
		{
			foreach (var aspect in m_aspects)
			{
				var httpResponse = await aspect.RequestReceivedAsync(httpRequest, cancellationToken).ConfigureAwait(false);
				if (httpResponse != null)
					return httpResponse;
			}
		}

		return null;
	}

	private async Task ResponseReadyAsync(HttpResponseMessage httpResponse, CancellationToken cancellationToken)
	{
		if (m_aspects != null)
		{
			foreach (var aspect in m_aspects)
				await aspect.ResponseReadyAsync(httpResponse, cancellationToken).ConfigureAwait(false);
		}
	}

	private HttpContentSerializer GetHttpContentSerializer(Type objectType) =>
		HttpServiceUtility.UsesBytesSerializer(objectType) ? m_bytesSerializer :
		HttpServiceUtility.UsesTextSerializer(objectType) ? m_textSerializer :
		m_contentSerializer;

	private static readonly IReadOnlyDictionary<string, string> s_emptyDictionary = new Dictionary<string, string>();
	private static readonly Regex s_regexPathParameterRegex = new(@"\{([a-zA-Z][a-zA-Z0-9]*)\}", RegexOptions.CultureInvariant);

	private readonly string m_rootPath;
	private readonly bool m_synchronous;
	private readonly HttpContentSerializer m_contentSerializer;
	private readonly HttpContentSerializer m_bytesSerializer;
	private readonly HttpContentSerializer m_textSerializer;
	private readonly IReadOnlyList<ServiceHttpHandlerAspect>? m_aspects;
	private readonly bool m_skipRequestValidation;
	private readonly bool m_skipResponseValidation;
	private readonly bool m_disableChunkedTransfer;
}
