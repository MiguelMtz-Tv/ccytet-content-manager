import axios from "axios";
import { Server } from "../libraries/servers";
import { Sessions } from "@/libraries/sessions";

export class ConvocatoriasService{
    baseUrl = Server.baseUrl

    create(data : any)
    {
        return axios.post<any>(this.baseUrl + 'api/convocatorias/create', data, Sessions.header())
    }

    dataSource(varArgs : any){
        return axios.post<any>(this.baseUrl + 'api/convocatorias/dataSource', varArgs, Sessions.header())
    }

    watch(id : string){
        let arg = { id : id }
        return axios.post<any>(this.baseUrl + 'api/convocatorias/watch', arg, Sessions.header())
    }
}