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

    addFiles(id: string, files: Array<any>){
        let data: any = {
            idEstadoSituacionFinanciera: id,
            files: files
        }
        return axios.post<any>(this.baseUrl + 'api/estadossituacionfinanciera/addfiles', data, Sessions.header())
    }

    delete(id : string){
        let data: any = {id : id}
        return axios.post<any>(this.baseUrl + 'api/estadossituacionfinanciera/delete', data, Sessions.header())
    }

    deleteFile(id: string){
        let data: any = { id : id }
        return axios.post<any>(this.baseUrl + 'api/estadossituacionfinanciera/deletefile', data, Sessions.header())
    }

}