<template>
    <div class="container mt-6 mb-7" v-if="invoiceData">
        <div class="row justify-content-center">
            <div class="col-lg-12 col-xl-7">
                <InvoiceDetailsCard :invoiceDataVal="invoiceData">
                    <button
                        @click="goBackAndEdit"
                        class="btn btn-warning btn-lg card-footer-btn justify-content-center text-uppercase-bold-sm hover-lift-light">
                        Go and Edit
                    </button>
                    <!--          <RouterLink :to="{name:'invoiceOrder', params: {id: route.params.id}}"-->
                    <!--                      class="btn btn-dark btn-lg card-footer-btn justify-content-center text-uppercase-bold-sm hover-lift-light"-->
                    <!--          >Edit-->
                    <!--          </RouterLink>-->
                    <button
                        @click="acceptInput"
                        class="btn btn-dark btn-lg card-footer-btn justify-content-center text-uppercase-bold-sm hover-lift-light">
                        Accept and Order
                    </button>
                </InvoiceDetailsCard>
            </div>
        </div>
    </div>
    <div v-else>Loading</div>
</template>

<script lang="ts" setup>
import {useIdentityStore} from "@/stores/identityStore";
import { useRouter, useRoute} from "vue-router";
import {onBeforeMount, ref} from "vue";
import type {IInvoice} from "@/dto/shop/IInvoice";
import InvoicesService from "@/services/shop/InvoicesService";
import InvoiceDetailsCard from "@/components/Shops/InvoiceDetailsCard.vue";
import {useMessageStore} from "@/stores/messageStore";
import type {IMessage} from "@/dto/shared/IMessage";
import {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";

const identitySore = useIdentityStore();
const invoicesService = new InvoicesService();
const messageStore = useMessageStore();


const route = useRoute();
const router = useRouter()
const invoiceData = ref<IInvoice>()

const acceptInput = async () => {
    let identity = identitySore.authenticationJwt;

    if (identity) {
        let result = (await invoicesService.acceptInvoice(identity, route.params.id as string, {acceptance: true}))

        if (result == 204) {
            console.log("Invoice status was changed")
            await router.push({name: 'invoiceOrder'});
        } else {
            let message: IMessage = {
                message: "Error occurred when accepting order", status: "",
                type: MessagePopupTypeEnum.Error
            }
            messageStore.addMessage(message)
            console.warn("Error occurred when accepting order")
        }
    } else {
        console.log("Identity problem")
    }
}

const goBackAndEdit = async () => {
    let identity = identitySore.authenticationJwt;

    if (identity) {
        let result = (await invoicesService.deleteInvoice(identity, route.params.id as string))

        if (result == 204) {
            console.log("Invoice was deleted")
            await router.push({name: 'businessDetails', params: {id: invoiceData.value!.businessId}});
        } else {
            console.warn("Error occurred when deleting invoice")
        }
    } else {
        console.log("Identity problem")
    }
}


onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();
    let identity = identitySore.authenticationJwt;

    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }

    if (route.params.id) {
        invoiceData.value = (await invoicesService.getInvoice(identity, route.params.id as string))
        console.log("Invoice details", invoiceData)
    } else {
        console.error("Invoice  id is not initialized")
    }

})

</script>