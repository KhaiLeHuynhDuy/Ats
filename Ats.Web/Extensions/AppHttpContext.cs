//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Features;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Ats.WebApp.Extensions
//{
//    public class AppHttpContext : DefaultHttpContext
//    {
//        DefaultHttpRequest _pooledHttpRequest;
//        DefaultHttpResponse _pooledHttpResponse;

//        public AppHttpContext(IFeatureCollection featureCollection) :
//            base(featureCollection)
//        {
//        }

//        protected override HttpRequest InitializeHttpRequest()
//        {
//            if (_pooledHttpRequest != null)
//            {
//                _pooledHttpRequest.Initialize(this);
//                return _pooledHttpRequest;
//            }

//            return new DefaultHttpRequest(this);
//        }

//        protected override void UninitializeHttpRequest(HttpRequest instance)
//        {
//            _pooledHttpRequest = instance as DefaultHttpRequest;
//            _pooledHttpRequest?.Uninitialize();
//        }

//        protected override HttpResponse InitializeHttpResponse()
//        {
//            if (_pooledHttpResponse != null)
//            {
//                _pooledHttpResponse.Initialize(this);
//                return _pooledHttpResponse;
//            }

//            return new DefaultHttpResponse(this);
//        }

//        protected override void UninitializeHttpResponse(HttpResponse instance)
//        {
//            _pooledHttpResponse = instance as DefaultHttpResponse;
//            _pooledHttpResponse?.Uninitialize();
//        }
//    }
//}
