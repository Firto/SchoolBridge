using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Services.Configuration;
using SchoolBridge.Helpers.AddtionalClases.ValidatingService;
using SchoolBridge.Domain.Managers.CClientErrorManager;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace SchoolBridge.Domain.Services.Implementation
{
    public delegate void ValidateFunc<T>(T prop, PropValidateContext context);
    public class ValidatingService: IValidatingService
    {
        private readonly IServiceProvider _provider;
        private static readonly IDictionary<string, object> _validateFunctions = new Dictionary<string, object>();

        public ValidatingService(IServiceProvider provider) {
            _provider = provider;
        }

        public static void OnInit(ClientErrorManager manager, IValidatingService validatingService) {
            manager.AddErrors(new ClientErrors("Validating", new Dictionary<string, ClientError>{
                    {"v-func-no", new ClientError("No validation function to property!")},
                    {"v-dto-invalid", new ClientError("Invalid dto!")}
                }));

            validatingService.AddValidateFunc("not-null", (object prop, PropValidateContext context) =>
            {
                if (prop == null)
                    context.Valid.Add($"[v-d-not-null, [pn-{context.PropName}]]");// $"Please input {context.PropName}!"
            });

            validatingService.AddValidateFunc("str-input", (string prop, PropValidateContext context) =>
            {
                if (string.IsNullOrEmpty(prop))
                    context.Valid.Add($"[v-d-not-null, [pn-{context.PropName}]]");// $"Please input {context.PropName}!"
            });

            validatingService.AddValidateFunc("str-guid", (string prop, PropValidateContext context) =>
            {
                if (prop == null) return;

                Guid x;
                if (!Guid.TryParse(prop, out x))
                    context.Valid.Add($"[v-inc-id, [pn-{context.PropName}]]");// $"Please input {context.PropName}!"
            });
        }

        public void Validate(string[] attrs, object obj, string objName)
        {
            IDictionary<string, IEnumerable<string>> valid = new Dictionary<string, IEnumerable<string>>();
            PropValidateContext _context = new PropValidateContext(_provider, null, null);
            _context.Valid = new List<string>();
            _context.PropName = objName.ToLower();
            foreach (var s in attrs)
                if (!_validateFunctions.ContainsKey(s))
                    throw new ClientException("v-func-no");
                else
                { 
                    try
                    {
                        if (obj is IEnumerable<object>) {
                            var m = (IEnumerable<object>)obj;
                            foreach (var item in m)
                                ((Delegate)_validateFunctions[s]).DynamicInvoke(item, _context);
                        }else ((Delegate)_validateFunctions[s]).DynamicInvoke(obj, _context);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }
                }

            if (_context.Valid.Count > 0)
            {
                valid.Add(objName, _context.Valid);
                _context.Valid = null;
            }

            if (valid.Count > 0)
                throw new ClientException("v-dto-invalid", valid);
        }

        public void Validate<T>(T dto) {
            Validate(typeof(T), dto);
        }

        public void Validate(Type type, object dto) {

            IDictionary<string, IEnumerable<string>> valid = new Dictionary<string, IEnumerable<string>>();
            PropValidAttribute temp;
            PropValidateContext _context = new PropValidateContext(_provider, type, dto);
            foreach (var item in type.GetProperties())
            {
                temp = (PropValidAttribute)item.GetCustomAttributes(false).FirstOrDefault(x => x.GetType() == typeof(PropValidAttribute));
                if (temp != null) {
                    if (_context.Valid == null)
                        _context.Valid = new List<string>();
                    _context.PropName = item.Name.ToLower();

                    foreach (var s in temp.FuncIdsAtributes)
                        if (!_validateFunctions.ContainsKey(s))
                            throw new ClientException("v-func-no");
                        else
                        {
                            try
                            {
                                if (item.GetValue(dto) is IEnumerable<object>)
                                {
                                    var m = (IEnumerable<object>)item.GetValue(dto);
                                    foreach (var r in m)
                                        ((Delegate)_validateFunctions[s]).DynamicInvoke(r, _context);
                                }
                                else ((Delegate)_validateFunctions[s]).DynamicInvoke(item.GetValue(dto), _context);
                            }
                            catch (TargetInvocationException e)
                            {
                                throw e.InnerException;
                            }
                        }

                    if (_context.Valid.Count > 0)
                    {
                        valid.Add(item.Name, _context.Valid);
                        _context.Valid = null;
                    }
                }
            }

            if (valid.Count > 0)
                throw new ClientException("v-dto-invalid", valid);
        }

        public bool IsIssetValidateFunc(string funcId)
            => _validateFunctions.ContainsKey(funcId);

        public void AddValidateFunc<T>(string funcId, ValidateFunc<T> func)
            => _validateFunctions.Add(funcId, func);

    }
}
