import { notification, type NotificationPlacement } from "ant-design-vue"

export default class CustomNotification{
    public error(message : string, position: NotificationPlacement = 'bottomRight'){
        notification.open({message: message, placement: position, style:{
            'border' : 'red solid 1px',
            'background' : '#fff2f0',
        }})
    }

    public warning(message : string, position: NotificationPlacement = 'bottomRight'){
        notification.open({message: message, placement: position, style:{
            'border' : 'red solid 1px',
            'background' : '#fff2f0',
        }})
    }

    public success(message : string, position: NotificationPlacement = 'bottomRight'){
        notification.open({message: message, placement: position, style:{
            'border' : 'red solid 1px',
            'background' : '#fff2f0',
        }})
    }

    public info(message : string, position: NotificationPlacement = 'bottomRight'){
        notification.open({message: message, placement: position, style:{
            'border' : 'red solid 1px',
            'background' : '#fff2f0',
        }})
    }
}