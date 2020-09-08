import { GlobalizationService } from '../Services/globalization.service';
import { FormBuilder, FormGroup } from '@angular/forms';



export function Globalization(prefix: string, constStrings: {errors: string[], validating: string[], args: string[]} | string[]) {
  return function _Globalization<T extends {new(...argss: any[]): {}}>(constr: T){
    return class extends constr {
      public form: FormGroup;

      constructor(...argss: any[]) {
        super(...argss);

        const _fb: FormBuilder = argss.find(x => x instanceof FormBuilder);
        if (_fb && 'args' in constStrings){
          const mm = {};
          constStrings.args.forEach(x => {
            mm[x] = [''];
          })
          this.form = _fb.group(mm);
        }

        const _gb: GlobalizationService = argss.find(x => x instanceof GlobalizationService);
        if (_gb){
          let constStringss;
          if ('errors' in constStrings){
            constStringss = [].concat(...constStrings.errors.map(x => 'cl-'+x),...constStrings.validating, ...constStrings.args.map(x => 'pn-' + x));
          }else constStringss = constStrings;
          _gb.initComponent(constr.name, prefix, constStringss);
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