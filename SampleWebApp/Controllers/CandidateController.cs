using DataTables;
using SampleWebApp.BLL;
using SampleWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace SampleWebApp.Controllers
{
    public class CandidateController : ApiController
    {
        IRepository<Candidate> _repository;
        public CandidateController()
        {
            this._repository = new Repository<Candidate>();  //add unity ioc later
        }

        /// <summary>
        /// Return candidate details
        /// </summary>
        /// <returns></returns>
        [Route("api/candidate"), HttpGet]
        public IHttpActionResult Get()
        {
            var model = _repository.GetAll();
            return Json(model);


        }


        [Route("api/candidate/delete"), HttpPost]
        public HttpResponseMessage Delete(int Id)
        {
            _repository.Delete(Id);
            _repository.Save();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        /// Deletes uploaded CV alone
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [Route("api/candidate/deletefile"), HttpGet]
        public HttpResponseMessage DeleteFile(int Id)
        {
            var candidateObj = _repository.GetById(Id);

            try
            {
                var folderPath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Uploads");
                var filePath = Path.Combine(folderPath, candidateObj.Resume);
                if (File.Exists(filePath))
                    File.Delete(filePath);

            }
            catch (Exception ex)
            {
                //throw custome exception
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            candidateObj.Resume = null;
            _repository.Update(candidateObj);
            _repository.Save();

            return Request.CreateResponse(HttpStatusCode.OK);
        }


        /// <summary>
        /// Common method for both save and update
        /// </summary>
        /// <returns></returns>
        [Route("api/candidate/save"), HttpPost]
        public HttpResponseMessage Save()
        {
            string rootPath = HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            var _request = HttpContext.Current.Request;
            bool _isCreate = false;

            int Id = Convert.ToInt32(_request.Form["Id"]);

            var candidateObj = _repository.GetById(Id);

            if (candidateObj == null)
            {
                candidateObj = new Candidate();
                _isCreate = true;
            }

            candidateObj.Id = candidateObj.Id;

            candidateObj.FirstName = _request.Form["FirstName"];
            candidateObj.LastName = _request.Form["LastName"];
            candidateObj.Email = _request.Form["Email"];
            candidateObj.Mobile = _request.Form["Mobile"];



            //uploaded files
            if (_request.Files.AllKeys.Any())
            {
                try
                {
                    var httpPostedFile = _request.Files["Resume"];
                    if (httpPostedFile != null)
                    {
                        var extension = Path.GetExtension(httpPostedFile.FileName);
                        var fileName = Path.GetFileNameWithoutExtension(httpPostedFile.FileName);

                        var storedFile = $"{fileName}_{DateTime.Now.Ticks}{extension}"; // to make uploaded files unique

                        var fileSavePath = Path.Combine(rootPath, storedFile);
                        httpPostedFile.SaveAs(fileSavePath);

                        candidateObj.Resume = storedFile;
                    }
                }
                catch (Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);

                }

            }


            if (_isCreate)
                _repository.Insert(candidateObj);
            else
                _repository.Update(candidateObj);


            _repository.Save();


            return Request.CreateResponse(HttpStatusCode.OK);
        }


    }
}
