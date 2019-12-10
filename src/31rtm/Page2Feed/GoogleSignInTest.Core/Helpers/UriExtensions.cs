using System;
using System.Collections.Generic;
using System.Web;

namespace GoogleSignInTest.Core.Helpers
{

    public static class UriExtensions
    {

        /// <summary>
        /// Adds the specified parameter to the Query String.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName">Name of the parameter to add.</param>
        /// <param name="paramValue">Value for the parameter to add.</param>
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Adds the specified parameter to the Query String.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters">Parameters to add.</param>
        /// <returns>Url with added parameter.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="parameters"/> is <see langword="null"/></exception>
        public static Uri AddParameters(
            this Uri url,
            IEnumerable<KeyValuePair<string, string>> parameters
            )
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var (key, value) in parameters)
            {
                query[key] = value;
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

        /// <summary>
        /// Adds the specified parameter to the Query String.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters">Parameters to add.</param>
        /// <returns>Url with added parameter.</returns>
        public static Uri AddParameters<T>(this Uri url, T parameters)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            var propertyInfos = parameters.GetType().GetProperties();
            foreach (var s in propertyInfos)
            {
                query[s.Name] = s.GetValue(parameters)?.ToString();
            }
            uriBuilder.Query = query.ToString();
            return uriBuilder.Uri;
        }

    }

}