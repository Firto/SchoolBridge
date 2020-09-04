import { GlobalizationService } from '../Services/globalization.service';



export function Globalization(prefix: string, constStrings: {errors: string[], validating: string[], args: string[]} | string[]) {
  return function _Globalization<T extends {new(...argss: any[]): {}}>(constr: T){
    return class extends constr {
      protected _defBackStringPrefix: string = prefix;
      constructor(...argss: any[]) {
        super(...argss);

        const _gb: GlobalizationService = argss.find(x => x instanceof GlobalizationService);
        if (_gb){
          if ('errors' in constStrings)
            constStrings = [].concat(...constStrings.errors.map(x => 'cl-'+x),...constStrings.validating, ...constStrings.args.map(x => 'pn-' + x));
            _gb.initComponent(constr.name, prefix, constStrings);
        }
        
      }
    } 
  }
}