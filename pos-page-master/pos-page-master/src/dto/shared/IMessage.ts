import type {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";

export  interface IMessage{
    status?: string,
    message?: string,
    type: MessagePopupTypeEnum
}