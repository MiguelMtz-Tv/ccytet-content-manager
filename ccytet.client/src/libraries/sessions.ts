import { AuthUtils } from "./auth.utils";

export class Sessions {
    public static authenticated: boolean = false
    public static accessToken : string

    public static start(jsonSession : any) : boolean {
        if(    
               jsonSession              != undefined && jsonSession             != null
            && jsonSession.id           != undefined && jsonSession.id          != null
            && jsonSession.token        != undefined && jsonSession.token       != null
            && jsonSession.expiration   != undefined && jsonSession.expiration  != null
            && jsonSession.nombre       != undefined && jsonSession.nombre      != null
            ) {
            
            this.setItem("id", jsonSession.id);
            this.setItem("token", jsonSession.token);
            this.setItem("expiration", jsonSession.expiration);
            this.setItem("nombre", jsonSession.nombre);

            this.accessToken = jsonSession.token
            this.authenticated = true
            return true;

        } else {
            //ELIMINAR SESION
            return false;
        }
    }

    public static getItem(key : string) {

        if (localStorage.getItem(key) !== undefined && localStorage.getItem(key) !== null) {
            try {
                let jsonValue : any = JSON.parse(localStorage.getItem(key) ?? "");
                return jsonValue;
            } catch (e) {
                return localStorage.getItem(key);
            }
        } else {
            return null;
        }
       
    }

    public static setItem(key : string, value : any) {
        try {
            let jsonValue : string = value;
            localStorage.setItem(key, jsonValue);
        } catch (e) {
            localStorage.setItem(key, value);
        }
    }

    public static validate() {
        if( Sessions.getItem("id") != null ) {
                return true;
        } else {
            return false;
        }
    }

    public static removeItem(key: string) {
        return localStorage.removeItem(key);
    }

    public static sessionDestroy() {
        var i = localStorage.length;
        while(i--) {
        var key = localStorage.key(i) ?? "";
            this.removeItem(key);
        }
    }

    public static header() {
        let objReturn = {
            headers : {
              'Content-Type'      : 'application/json',
              'Authorization'     : 'Bearer ' + Sessions.getItem("token")
            }
        };

        return objReturn;
    }

    public static headerFormData() {
        let objReturn = {
            headers : {
              'Authorization'       : 'Bearer ' + Sessions.getItem("token")
            }
        };

        return objReturn;
    }

    public static headerFile() {
        let objReturn = {
            responseType : 'blob' as 'json',
            headers : {
                'Authorization'       : 'Bearer ' + Sessions.getItem("token")
            }
        };

        return objReturn;
    }

    public static headerFileNotToken() {
        let objReturn = {
            responseType : 'blob' as 'json'
        };

        return objReturn;
    }

    public static check(){
        this.accessToken = this.getItem('token')
      
       // Check the access token availability
        if (!this.accessToken) {
            return false;
        }

       // Check the access token expire date
        if (AuthUtils.isTokenExpired(this.accessToken)) {
            return false;
        }

        // Check if the user is logged in
        if (AuthUtils._decodeToken(this.accessToken).valid) {
            return true;
        }

      
        //If the access token exists and it didn't expire, sign in using it
        //return this.signInUsingToken()
        return true
    }
      
    getTokenExpiration() : string {
        let expiration = localStorage.getItem('expiration') || ''
        return expiration
    }
}
