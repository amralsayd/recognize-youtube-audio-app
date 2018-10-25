using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YoutubeTool.Business;
using YoutubeTool.Business.ViewModels;

namespace YoutubeTool.Presentaion.Website.Controllers
{
    public class HomeController : Controller
    {
        ILog log = log4net.LogManager.GetLogger(typeof(HomeController)); 
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ParseUrl(SearchFormViewModel searchFormViewModel)
        {
            try
            {
                YoutubeLogic YLogic = new YoutubeLogic();
                VideoViewModel videoViewModel = await YLogic.GetVideoInfoByUrl(searchFormViewModel.youtubeUrl);
                return Json(new
                {
                    status = true,
                    videoViewModel = videoViewModel,
                    partialViewData = RenderRazorViewToString("_VideoResult", videoViewModel)
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> DownloadAudioFile(SearchFormViewModel searchFormViewModel)
        {
            try
            { 
                YoutubeLogic YLogic = new YoutubeLogic();
                AudioFileViewModel audioFileViewModel =await YLogic.DownloadAudioByUrl(searchFormViewModel.youtubeUrl);
                string siteBaseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                audioFileViewModel.fileWebsitePath = siteBaseUrl + "Downloads/Audio/" + audioFileViewModel.fileName;
                return Json(new 
                { 
                    status = true,
                    audioFileViewModel = audioFileViewModel,
                    partialViewData = RenderRazorViewToString("_AudioResult", audioFileViewModel) 
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SplitAudioFile(AudioFileViewModel audioFileViewModel)
        {
            try
            { 
                YoutubeLogic YLogic = new YoutubeLogic();
                AudioFileViewModel splitAudioFileViewModel = YLogic.SplitAudioFile(audioFileViewModel);
                string siteBaseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
                splitAudioFileViewModel.fileWebsitePath = siteBaseUrl + "Downloads/Audio/" + splitAudioFileViewModel.fileName;
                return Json(new
                {
                    status = true,
                    audioFileViewModel = splitAudioFileViewModel,
                    partialViewData = RenderRazorViewToString("_AudioResult", splitAudioFileViewModel)
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false,message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<JsonResult> RecognizeAudioFile(AudioFileViewModel audioFileViewModel)
        {
            try
            { 
                YoutubeLogic YLogic = new YoutubeLogic();
                ArtistViewModel artistViewModel = await YLogic.RecognizeAudioFile(audioFileViewModel);
                return Json(new
                {
                    status = true,
                    artistViewModel = artistViewModel,
                    partialViewData = RenderRazorViewToString("_ArtistResult", artistViewModel)
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false,message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SerachYoutube(ArtistViewModel artistViewModel)
        {
            try
            {
                YoutubeLogic YLogic = new YoutubeLogic();
                List<SnippetViewModel> snippetViewModelList = await YLogic.SearchYoutubeSnippets(artistViewModel.artist);
                return Json(new
                {
                    status = true,
                    snippetViewModelList = snippetViewModelList,
                    partialViewData = RenderRazorViewToString("_SearchResult", snippetViewModelList)
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Json(new { status = false,message = ex.Message });
            }
        }

        //we can add this method in base controller to reuse it in all website
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}