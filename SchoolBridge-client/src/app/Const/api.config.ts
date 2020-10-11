import { Service } from "src/app/Interfaces/Service/service.interface";

export const apiConfig: Record<string, Service> = {
  login: {
    url: "login/",
    methods: {
      login: {
        url: "",
        type: "POST",
      },
      logout: {
        url: "logout/",
        type: "GET",
      },
      refreshtoken: {
        url: "refreshtoken/",
        type: "POST",
        loader: false,
      },
    },
  },
  register: {
    url: "register/",
    methods: {
      start: {
        url: "start",
        type: "GET",
      },
      end: {
        url: "end/",
        type: "POST",
      },
      forgotpassword: {
        url: "changepassword/",
        type: "GET",
      },
      endforgotpassword: {
        url: "endchangepassword/",
        type: "POST",
      },
    },
  },
  notification: {
    url: "notification/",
    methods: {
      read: {
        url: "read",
        type: "GET",
        loader: false,
      },
      get: {
        url: "get",
        type: "GET",
        loader: false,
      },
      getandread: {
        url: "getandread",
        type: "GET",
        loader: false,
      },
      getcountunread: {
        url: "getcountunread",
        type: "GET",
        loader: false,
      },
    },
  },
  globalization: {
    url: "globalization/",
    methods: {
      info: {
        url: "info",
        type: "GET",
        loader: false,
      },
      strings: {
        url: "strings",
        type: "POST",
        loader: false,
      },
      "add-language": {
        url: "language/add",
        type: "POST",
      },
      "add-or-upd-string": {
        url: "string/addorupdate",
        type: "POST",
        loader: false,
      },
      "remove-language": {
        url: "language/remove",
        type: "GET",
      },
      "update-baseupdateid": {
        url: "update-baseupdateid",
        type: "GET",
      },
    },
  },
  profile: {
    url: "profile/",
    methods: {
      info: {
        url: "info",
        type: "GET",
      },
      "change-login": {
        url: "changeLogin",
        type: "POST",
      },
      "change-image": {
        url: "changeImage",
        type: "POST",
      }
    },
  },
  users: {
    url: "user/",
    methods: {
      get: {
        url: "get",
        type: "GET",
      },
      getMany: {
        url: "getmany",
        type: "POST",
        loader: false,
      },
      getAll: {
        url: "getAll",
        type: "GET",
        loader: false,
      },
      ban: {
        url: "ban",
        type: "POST",
        loader: false,
      },
      unban: {
        url: "unban",
        type: "POST",
        loader: false,
      },
    },
  },
  chat: {
    url: "chat/",
    methods: {
      getChats: {
        url: "getchats",
        type: "GET",
        loader: false,
      },
      getMessages: {
        url: "getMessages",
        type: "GET",
        loader: false,
      },
      sendMessage: {
        url: "sendMessage",
        type: "GET",
        loader: false,
      },
    },
  },
};
