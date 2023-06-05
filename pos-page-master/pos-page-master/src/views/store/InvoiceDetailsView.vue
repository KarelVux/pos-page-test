<template>
    <div class="container mt-6 mb-7" v-if="invoiceData">
        <div class="row justify-content-center">
            <div class="col-lg-12 col-xl-7">
                <InvoiceDetailsCard :invoiceDataVal="invoiceData">
                    <button
                        @click="router.go(-1)"
                        class="btn btn-primary btn-lg card-footer-btn justify-content-center text-uppercase-bold-sm hover-lift-light">
                        Go back
                    </button>
                </InvoiceDetailsCard>
                <div class="card mt-2" >
                    <div class="card-body p-5" v-if="orderFeedbackData">
                        <h2>
                            Here is the previously submitted order feedback
                        </h2>
                        <div>
                            <p v-if="orderFeedbackData.title"><strong>Title</strong>: {{ orderFeedbackData.title }}</p>
                            <p><strong>Rating</strong>: {{ orderFeedbackData.rating }}</p>
                            <p><strong>Description</strong></p>
                            <p>{{ orderFeedbackData.description }}</p>
                        </div>
                    </div>
                    <div v-else  class="card-body p-5">
                        <h2>Submit your feedback</h2>
                        <div class="row g-3 align-items-center">
                            <label class="form-label">Title</label>
                            <input type="text" v-model="feedbackCreationData.title" class="form-control">
                        </div>
                        <div class="row g-3 align-items-center">
                            <label class="form-label">Description</label>
                            <input type="text" v-model="feedbackCreationData.description" class="form-control">
                        </div>
                        <div class="row g-3 align-items-center">
                            <label class="form-label">Rating</label>
                            <input type="number" min=" 0" max="10" v-model="feedbackCreationData.rating"
                                   class="form-control">
                        </div>
                        <button type="button" class="btn btn-secondary" @click="submitData">Submit</button>
                    </div>
                </div>

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
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";
import OrderFeedbackService from "@/services/shop/OrderFeedbackService";
import type {IOrderFeedback} from "@/dto/shop/IOrderFeedback";

const identitySore = useIdentityStore();
const invoicesService = new InvoicesService();
const orderFeedbckService = new OrderFeedbackService();

const route = useRoute();
const router = useRouter()
const invoiceData = ref<IInvoice>()
const orderFeedbackData = ref<IOrderFeedback | undefined>()

const feedbackCreationData = ref<IOrderFeedback>({
    title: "",
    description: "",
    rating: 0,
    orderId: "",
} as IOrderFeedback);

onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();

    invoiceData.value = {} as IInvoice
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

    if (invoiceData.value?.order?.id) {
        orderFeedbackData.value = await orderFeedbckService.getOrderFeedbackViaOrderId(identity, invoiceData.value?.order?.id)
    }
})

const submitData = async () => {
    let identity = identitySore.authenticationJwt;
    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }

    if (route.params.id) {
        feedbackCreationData.value.orderId = invoiceData.value?.order?.id!;
        (await orderFeedbckService.createOrderFeedback(identity, feedbackCreationData.value));
        orderFeedbackData.value = await orderFeedbckService.getOrderFeedbackViaOrderId(identity, invoiceData.value?.order?.id!)
        console.log("Order feedback details", orderFeedbackData.value)

    } else {
        console.error("Order feedback details")
    }
}
</script>