import * as some from '../Helpers/device-uuid.js';
export class DeviceUUIDService{
    private du: any = new some.DeviceUUID().parse();
    
    get():string{
        const dua = [
            this.du.language,
            this.du.platform,
            this.du.os,
            this.du.cpuCores,
            this.du.isAuthoritative,
            this.du.silkAccelerated,
            this.du.isKindleFire,
            this.du.isDesktop,
            this.du.isMobile,
            this.du.isTablet,
            this.du.isWindows,
            this.du.isLinux,
            this.du.isLinux64,
            this.du.isMac,
            this.du.isiPad,
            this.du.isiPhone,
            this.du.isiPod,
            this.du.isSmartTV,
            this.du.pixelDepth,
            this.du.isTouchScreen,
            this.du.browser,
            this.du.resolution
        ];
        return this.du.hashMD5(dua.join(':'));
    }
}