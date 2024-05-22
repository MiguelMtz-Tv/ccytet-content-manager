import axios from "axios";
import { Server } from "../libraries/servers";
import { Sessions } from "@/libraries/sessions";

export class EsfService{
    baseUrl = Server.baseUrl

    create(period : Date){
        let data: any = {periodo: period}
        return axios.post<any>(this.baseUrl + 'api/estadossituacionfinanciera/create', data)
    }
}