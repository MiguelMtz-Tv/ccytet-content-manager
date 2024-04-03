import axios from "axios";
import { Server } from "../libraries/servers";
import { Sessions } from "@/libraries/sessions";
import { AuthUtils } from "@/libraries/auth.utils";

export class AuthService{
    baseUrl = Server.baseUrl
    protected authenticated!: boolean;

    set accessToken(token: string) {
        localStorage.setItem('access_token', token);
    }

    get accessToken(): string {
        return localStorage.getItem('access_token') ?? "";
    }

    public signIn(data : any){
        return axios.post<any>(this.baseUrl + 'api/aspNetUser/signIn', data)
    }

    public create(data : any){
        return axios.post<any>(this.baseUrl + 'api/aspNetUser/create', data)
    }

    signOut(){
        localStorage.removeItem('acces_token')
        this.authenticated = false
        Sessions.sessionDestroy();
    }
      
}