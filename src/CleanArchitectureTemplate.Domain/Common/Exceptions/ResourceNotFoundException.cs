using System;

namespace CleanArchitectureTemplate.Domain.Common.Exceptions
{
    public class ResourceNotFoundException : Exception, IBusinessException
    {
    }
}