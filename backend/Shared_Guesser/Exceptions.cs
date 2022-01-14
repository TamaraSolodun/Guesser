using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json.Serialization;

namespace Shared_Guesser
{
    /// <summary>
    ///     ExtProblemDetails
    /// </summary>
    public class ExtProblemDetails : ProblemDetails
    {
        /// <summary>
        ///     Type
        /// </summary>
        [JsonIgnore]
        public new string Type { get; set; }
    }

    /// <summary>
    ///     NotFoundException
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {

        }
    }

    /// <summary>
    ///     NotFoundException
    /// </summary>
    public class UnacceptableStepException : Exception
    {
        public UnacceptableStepException(string message) : base(message)
        {

        }
    }

    /// <summary>
    ///     BadRequestException
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message)
        {

        }
    }

    /// <summary>
    ///     AccessDeniedException
    /// </summary>
    public class PaymentRequiredException : Exception
    {
        public PaymentRequiredException(string message) : base(message)
        {

        }
    }

    /// <summary>
    ///     AccessDeniedException
    /// </summary>
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException(string message) : base(message)
        {

        }
    }

    /// <summary>
    ///     DuplicateEntityException
    /// </summary>
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string message) : base(message)
        {

        }
    }

    /// <summary>
    ///     NoRightsException
    /// </summary>
    public class NoRightsException : Exception
    {
        public NoRightsException(string message) : base(message)
        {

        }
    }
}
