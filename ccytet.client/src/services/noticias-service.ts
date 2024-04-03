import axios from "axios";
import { Server } from "../libraries/servers";
import { Sessions } from "@/libraries/sessions";
import { AuthUtils } from "@/libraries/auth.utils";
import { genActionStyle } from "ant-design-vue/es/alert/style";

export class NoticiasService {
    baseUrl = Server.baseUrl

    public create(data : any){
        return axios.post<any>(this.baseUrl + 'api/noticias/create', data, Sessions.header())
    }
}