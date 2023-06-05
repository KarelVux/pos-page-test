import {useIdentityStore} from "@/stores/identityStore";
import {useMessageStore} from "@/stores/messageStore";
import {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";
import {useRouter} from "vue-router";


export async function redirectUserIfIdentityTokenIsNull(): Promise<void> {

    const identityStore = useIdentityStore();
    const messageStore = useMessageStore();
    const identity = identityStore.authenticationJwt
    const router = useRouter();
    if (identity === undefined) {
        messageStore.addMessage({
            status: "Redirected to home page",
            message: "Please login to access this page!",
            type: MessagePopupTypeEnum.Info
        })
        await router.push({name: 'home'})
    }
}