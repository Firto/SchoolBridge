import { Service } from 'src/app/Interfaces/Service/service.interface';

export const apiConfig: Record<string, Service> = {
    "login" : {
        url: "login/",
        methods: {
            "login": {
                url: "",
                type: "POST"
            },
            "logout": {
                url: "logout/",
                type: "GET"
            },
            "refreshtoken": {
                url: "refreshtoken/",
                type: "POST"
            }
        }
    },
    "register": {
        url: "register/",
        methods: {
            "start": {
                url: "start",
                type: "GET"
            },
            "end": {
                url: "end/",
                type: "POST"
            }
        }
    },
    "notification": {
        url: "notification/",
        methods: {
            "read": {
                url: "read",
                type: "GET",
                loader: false
            },
            "get": {
                url: "get",
                type: "GET",
                loader: false
            },
            "getandread": {
                url: "getandread",
                type: "GET",
                loader: false
            },
            "getcountunread": {
                url: "getcountunread",
                type: "GET",
                loader: false
            }
        }
    },
    "globalization": {
        url: "globalization/",
        methods: {
            "info": {
                url: "info",
                type: "GET"
            },
            "strings": {
                url: "strings",
                type: "GET",
                loader: false
            },
            "add-language": {
                url: "language/add",
                type: "POST"
            },
            "remove-language": {
                url: "language/remove",
                type: "GET"
            }
        }
    }
};