<template>
    <!-- Steps form -->

    <div class="container">

        <div class="card">
            <div class="card-body mb-4" v-if="businessLimitedData && invoiceData && invoiceData.order">
                <h2 class="card-title text-center py-2">
                    Here is you order status from {{ businessLimitedData.name }}
                </h2>
                <OrderProgressStatus
                    v-if="invoiceData.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessRejected"
                    header="Sorry but business has rejected your order and your order has been canceled"
                    strongText=""
                    progressbarWidth="0"
                />

                <OrderProgressStatus
                    v-else-if="invoiceData.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Ready"
                    header="Your order is ready for pickup"
                    strongText=""
                    progressbarWidth="75"
                />
                <OrderProgressStatus
                    v-else-if="invoiceData.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.BusinessIsPreparing"
                    header="Business owner has accepted the order and preparing it"
                    strongText=""
                    progressbarWidth="50"
                />
                <OrderProgressStatus
                    v-else-if="invoiceData.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Unknown"
                    header="Business owner is checking the order"
                    strongText="Please wait"
                    progressbarWidth="25"
                />

                <div v-if="businessLimitedData">
                    <BusinessIntroduction :businessDetails="businessLimitedData"/>
                </div>

                <div v-if="invoiceData">
                    <InvoiceDetailsCard :invoiceDataVal="invoiceData"/>
                </div>
            </div>
            <div v-else>
                <LoadingData/>
            </div>
        </div>
    </div>

</template>


<script lang="ts" setup>
import {useIdentityStore} from "@/stores/identityStore";
import {useRoute} from "vue-router";
import {onBeforeMount, onMounted, onUnmounted, ref} from "vue";
import InvoicesService from "@/services/shop/InvoicesService";
import type {IInvoice} from "@/dto/shop/IInvoice";
import ShopsService from "@/services/shop/ShopsService";
import type {IBusiness} from "@/dto/shop/IBusiness";
import OrderProgressStatus from "@/components/Shops/Elements/OrderProgressStatus.vue";
import BusinessIntroduction from "@/components/Shops/BusinessIntroduction.vue";
import InvoiceDetailsCard from "@/components/Shops/InvoiceDetailsCard.vue";
import {OrderAcceptanceStatusEnum} from "@/dto/enums/OrderAcceptanceStatusEnum";
import LoadingData from "@/components/shared/LoadingData.vue";
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";
import type {IMessage} from "@/dto/shared/IMessage";
import {InvoiceAcceptanceStatusEnum} from "@/dto/enums/InvoiceAcceptanceStatusEnum";
import {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";
import {useMessageStore} from "@/stores/messageStore";

const identitySore = useIdentityStore();
const messageStore = useMessageStore();

const invoicesService = new InvoicesService();
const shopsService = new ShopsService();

const businessLimitedData = ref<IBusiness>()
const invoiceData = ref<IInvoice>()

const props = defineProps({
    id: String,
})

const message = ref<IMessage>()

const route = useRoute();
let timerId: number;

onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();

    let identity = identitySore.authenticationJwt;
    let id = route.params.id
    console.log("Invoice details id is recieved", id)
    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }

    if (id) {
        invoiceData.value = (await invoicesService.getInvoice(identity, route.params.id.toString()))
        businessLimitedData.value = await shopsService.getBusinessInfo(identity, invoiceData.value!.businessId)
    } else {
        console.error("Invoice id is not initialized")
    }

})

onMounted(() => {
    let identity = identitySore.authenticationJwt;

    timerId = setInterval(async () => {

        if (identity && props.id)
            invoiceData.value = await invoicesService.getInvoice(identity, props.id)
            if (invoiceData.value?.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessRejected && invoiceData.value?.order?.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Closed ){
                message.value = {
                    message: "Business rejected your order",
                    type: MessagePopupTypeEnum.Error
                }
                    messageStore.addMessage(message.value!)
                clearInterval(timerId);
            }
    }, 15000);
});

// call out to remove memory leaks?
onUnmounted(() => {
    clearInterval(timerId);
});
</script>