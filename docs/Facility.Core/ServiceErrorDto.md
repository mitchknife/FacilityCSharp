# ServiceErrorDto class

An error.

An error.

```csharp
public sealed class ServiceErrorDto : ServiceDto<ServiceErrorDto>
```

## Public Members

| name | description |
| --- | --- |
| [ServiceErrorDto](ServiceErrorDto/ServiceErrorDto.md)() | Creates an instance. |
| [ServiceErrorDto](ServiceErrorDto/ServiceErrorDto.md)(…) | Creates a service error. (2 constructors) |
| [Code](ServiceErrorDto/Code.md) { get; set; } | The error code. |
| [DetailsObject](ServiceErrorDto/DetailsObject.md) { get; set; } | Advanced error details. |
| [InnerError](ServiceErrorDto/InnerError.md) { get; set; } | The inner error. |
| [Message](ServiceErrorDto/Message.md) { get; set; } | The error message. |
| override [IsEquivalentTo](ServiceErrorDto/IsEquivalentTo.md)(…) | Determines if two DTOs are equivalent. |
| override [ToString](ServiceErrorDto/ToString.md)() | Returns the DTO as JSON. |

## See Also

* class [ServiceDto&lt;T&gt;](./ServiceDto-1.md)
* namespace [Facility.Core](../Facility.Core.md)
* [ServiceErrorDto.cs](https://github.com/FacilityApi/FacilityCSharp/tree/master/src/Facility.Core/ServiceErrorDto.cs)

<!-- DO NOT EDIT: generated by xmldocmd for Facility.Core.dll -->
