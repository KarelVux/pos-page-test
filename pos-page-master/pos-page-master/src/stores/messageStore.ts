import {ref} from 'vue';
import {defineStore} from 'pinia';
import type {IMessage} from "@/dto/shared/IMessage";

export const useMessageStore = defineStore('message', () => {
    const messages = ref<IMessage[]>([])

    const addMessage = (message: IMessage) => {
        if (message) {
            messages.value.push(message)
        }
    }


    const messageExists = (): boolean => {
        return messages.value.length > 0;
    }

    const getLastMessage = (): IMessage | undefined => {
        if (messageExists()) {
            return messages.value.slice(-1)[0]
        } else {
            return undefined
        }

    }

    const getAllMessages = (): IMessage[] | undefined => {
        if (messageExists()) {
            return messages.value.slice()
        } else {
            return undefined
        }

    }


    const removeLastAddedMessage = (): IMessage | undefined => {
        const message: IMessage | undefined = messages.value.pop();

        if (message)
            return message;
        else
            return undefined;
    }
    // delete element if searchable element is found
    const removeSpecifiedMessage = (removeSpecifiedMessage: IMessage): void => {
        const index = messages.value.findIndex((item: IMessage) => item.status === removeSpecifiedMessage.status && item.message === removeSpecifiedMessage.message);
        if (index !== -1) {
            messages.value.splice(index, 1);
        }
    }


    return {
        addMessage,
        messageExists,
        removeLastAddedMessage,
        getLastMessage,
        getAllMessages,
        removeSpecifiedMessage
    };
});


