using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;

namespace DanLiris.Admin.Web
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var rawErrors = context.ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault());

                var errors = new Dictionary<string, object>();

                foreach (var e in rawErrors)
                {
                    var keys = e.Key.Split(".");

                    SetValues(keys, 0, e.Value, errors);
                }

                var responseObj = new
                {
                    message = "data does not pass validation",
                    error = errors,
                    statusCode = 400
                };

                context.Result = new JsonResult(responseObj)
                {
                    StatusCode = 400
                };
            }
        }

        private void SetValues(string[] keys, int keyIndex, string value, IDictionary<string, object> parentDic)
        {
            var key = keys[keyIndex];

            if (keys.Length > keyIndex + 1)
            {
                object childObj;
                IDictionary<string, object> childDict = new Dictionary<string, object>();
                List<object> childList = new List<object>();

                var indexOfOpenBracket = key.IndexOf("[");
                var indexOfCloseBracket = key.IndexOf("]");

                if (indexOfOpenBracket > -1 && indexOfCloseBracket > -1)
                {
                    var index = int.Parse(key.Substring(indexOfOpenBracket + 1, indexOfCloseBracket - indexOfOpenBracket - 1));
                    key = key.Substring(0, indexOfOpenBracket);

                    if (parentDic.TryGetValue(key, out childObj))
                    {
                        childList = (List<object>)childObj;

                        if (childList.Count - 1 == index)
                        {
                            childDict = (IDictionary<string, object>)childList[index];
                        }
                        else
                        {
                            for (int i = childList.Count; i < index; i++)
                            {
                                childList.Add(null);
                            }
                            childList.Add(childDict);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < index; i++)
                        {
                            childList.Add(null);
                        }
                        childList.Add(childDict);

                        parentDic[key] = childList;
                    }
                }
                else
                {
                    if (parentDic.TryGetValue(key, out childObj))
                    {
                        childDict = (IDictionary<string, object>)childObj;
                    }
                    else
                    {
                        parentDic[key] = childDict;
                    }
                }

                SetValues(keys, keyIndex + 1, value, childDict);

            }
            else
            {
                parentDic[key] = value;
            }
        }

    }
}