import * as some from './device-uuid.js';
export class DeviceUUIDService{
    private du: any = new some.DeviceUUID().parse();
    
    get():string{
        const dua = [
            this.du.version,
            this.du.language,
            this.du.platform,
            this.du.os,
            this.du.pixelDepth,
            this.du.colorDepth,
            this.du.resolution,
            this.du.isAuthoritative,
            this.du.silkAccelerated,
            this.du.isKindleFire,
            this.du.isDesktop,
            this.du.isMobile,
            this.du.isTablet,
            this.du.isWindows,
            this.du.isLinux,
            this.du.isLinux64,
            this.du.isChromeOS,
            this.du.isMac,
            this.du.isiPad,
            this.du.isiPhone,
            this.du.isiPod,
            this.du.isAndroid,
            this.du.isSamsung,
            this.du.isSmartTV,
            this.du.isRaspberry,
            this.du.isBlackberry,
            this.du.isTouchScreen,
            this.du.isOpera,
            this.du.isIE,
            this.du.isEdge,
            this.du.isIECompatibilityMode,
            this.du.isSafari,
            this.du.isFirefox,
            this.du.isWebkit,
            this.du.isChrome,
            this.du.isKonqueror,
            this.du.isOmniWeb,
            this.du.isSeaMonkey,
            this.du.isFlock,
            this.du.isAmaya,
            this.du.isPhantomJS,
            this.du.isEpiphany,
            this.du.source,
            this.du.cpuCores,
        ];
        return this.du.hashMD5(dua.join(':'));
    }
}