import axios from "axios";
import { Server } from "../libraries/servers";
import { Sessions } from "@/libraries/sessions";

export class EsfService{
    baseUrl = Server.baseUrl

    create(period : Date){
        let data: any = {periodo: period}
        return axios.post<any>(this.baseUrl + 'api/estadossituacionfinanciera/create', data, Sessions.header())
    }

    index(year : number){
        let data: any = {year: year}
        return axios.post<any>(this.baseUrl + 'api/estadossituacionfinanciera/index', data, Sessions.header())
    }
}