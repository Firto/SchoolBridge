import { Injectable } from '@angular/core';
import { CryptService } from './crypt.service';
import { DeviceUUIDService } from './device-uuid.service';

@Injectable()
export class MyLocalStorageService {
    private _data: Record<string, any> = {};

    constructor(private _cryptService: CryptService,
                private _uuidService: DeviceUUIDService) {
        if (localStorage.getItem("data")){
            try{
                this._data = JSON.parse(this._cryptService.decode(localStorage.getItem('data'), this._uuidService.uuid))
            }catch{
                localStorage.removeItem("data");
                localStorage.setItem("data", this._cryptService.encode(JSON.stringify(this._data), this._uuidService.uuid));
            }
        }        
    }

    public write<T>(name: string, obj: T){
        this._data[name] = obj;
        localStorage.setItem("data", this._cryptService.encode(JSON.stringify(this._data), this._uuidService.uuid));
    }

    public read<T>(name: string): T{
        return this.isIssetKey(name) ? this._data[name] : null;
    }

    public isIssetKey(name: string): Boolean{
        return Object.keys(this._data).includes(name) && this._data[name] != null;
    }

    public remove(name: string){
        delete this._data[name];
        localStorage.setItem("data", this._cryptService.encode(JSON.stringify(this._data), this._uuidService.uuid));
    }
}