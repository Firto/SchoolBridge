import { AES, enc } from 'crypto-ts';
import { Injectable } from '@angular/core';

@Injectable()
export class CryptService {
    /*encode(text:string, key:string): string {
        var j=0;
        var str = "";
        key = this.utf8_encode(key);
        text = this.utf8_encode(text);
        for (var i=0; i<text.length; i++) {
            var a = text.charCodeAt(i);
            var b = key.charCodeAt(j);
            var c = a + b;
            if (c > 255) c -= 255;
            str += String.fromCharCode(c);
            if (j == key.length-1) j=0; else j++;
        }
        return window.btoa(str);
    }*/

    encode(text: string, key: string): string{
        return AES.encrypt(text, key).toString();
    }

    decode(text: string, key: string): string{
        return AES.decrypt(text, key).toString(enc.Utf8);
    } 

    /*decode(text:string, key:string): string {
        var j=0;
        var str = "";
        key = this.utf8_encode(key);
        text = window.atob(text);
        for (var i=0; i<text.length; i++) {
            var a = text.charCodeAt(i);
            var b = key.charCodeAt(j);
            var c = a - b;
            if (c < 0) c += 255;
            str += String.fromCharCode(c);
            if (j == key.length-1) j=0; else j++;
        }
        return this.utf8_decode(str);
    }*/

    private utf8_encode(text: string): string {
        let str = "";
        for (var i=0; i<text.length; i++) {
            if (text.charCodeAt(i) > 255) 
                str += String.fromCharCode(text.charCodeAt(i)-848);
            else str += text.charAt(i);
        }
        return str;
    }

    private utf8_decode(text: string): string {
        let str = "";
        for (var i=0; i<text.length; i++) {
            if (text.charCodeAt(i) > 127) 
                str += String.fromCharCode(text.charCodeAt(i)+848);
            else str += text.charAt(i);
        }
        return str;
    }
}