import { FormBuilder, FormGroup } from '@angular/forms';

export function NgxForm(args: string[]) {
    return function _NgxForm<T extends {new(...argss: any[]): {}}>(constr: T){
      return class extends constr {
        public form: FormGroup;
  
        constructor(...argss: any[]) {
          super(...argss);
  
          const _fb: FormBuilder = argss.find(x => x instanceof FormBuilder);
          if (args){
            const mm = {};
            args.forEach(x => {
              mm[x] = [''];
            })
            this.form = _fb.group(mm);
          }
        }
  
        protected validate(err: any){
          if (err.additionalInfo)
            for (const [key, value] of Object.entries(err.additionalInfo)) 
              this.form.controls[key].setErrors({'err': err.additionalInfo[key]});
        }
      } 
    }
  }