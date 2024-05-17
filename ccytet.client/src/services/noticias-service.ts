import axios from "axios";
import { Server } from "../libraries/servers";
import { Sessions } from "@/libraries/sessions";

export class NoticiasService {
    baseUrl = Server.baseUrl

    public create(data : any){
        return axios.post<any>(this.baseUrl + 'api/noticias/create', data, Sessions.header())
    }

    public update(data : any){
        return axios.post<any>(this.baseUrl + 'api/noticias/update', data, Sessions.header())
    }

    public dataSource(varArgs : any)
    {
        return axios.post<any>(this.baseUrl + 'api/noticias/dataSource', varArgs, Sessions.header())
    }

    public watch(id:string){
        let varArgs = { id: id }
        return axios.post<any>(this.baseUrl + 'api/noticias/watch', varArgs, Sessions.header())
    }
}