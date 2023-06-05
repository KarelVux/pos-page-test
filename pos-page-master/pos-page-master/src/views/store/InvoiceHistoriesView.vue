<template>
    <div class="container">


        <div class="card my-2" v-if="openInvoices.length > 0">
            <div class="card-body mb-4">
                <h2 class="card-title text-center py-2">
                    Here are your active orders
                </h2>
                <table class="table border-bottom border-gray-200 mt-3">
                    <thead>
                    <tr>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm px-0">Invoice ID</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm x px-0">Business name</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Order date</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Final price</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="openInvoice in openInvoices " :key="openInvoice.id">
                        <td @click="openInvoiceOrderDetails(openInvoice.id!)"
                            class="px-0 custom-link"
                        >{{ openInvoice.id }}
                        </td>
                        <td class=" px-0" v-if="openInvoice.businessName">{{ openInvoice.businessName }}</td>
                        <td class=" px-0" v-else>{{ openInvoice.businessId }}</td>
                        <td class="text-end px-0">
                            {{ getFormattedDate(openInvoice.creationTime) }}
                        </td>
                        <td class="text-end px-0">{{ openInvoice.finalTotalPrice }}</td>
                        <td class="text-end px-0">{{ openInvoice.finalTotalPrice }}</td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="card my-2">
            <div class="card-body mb-4">
                <h2 class="card-title text-center py-2">
                    Here are your old orders
                </h2>
                <table class="table border-bottom border-gray-200 mt-3" v-if="closedInvoices.length > 0">
                    <thead>
                    <tr>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm px-0">Invoice ID</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-0">Business name</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Order date</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Final price</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Was successfully
                            completed
                        </th>

                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="closedInvoice in closedInvoices " :key="closedInvoice.id">
                        <td class="px-0">

                            <RouterLink :to="{name: 'invoiceDetails', params: {id: closedInvoice.id}}">
                                {{
                                    closedInvoice.id
                                }}
                            </RouterLink>
                        </td>
                        <td class=" px-0" v-if="closedInvoice.businessName">{{ closedInvoice.businessName }}</td>
                        <td class=" px-0" v-else>{{ closedInvoice.businessId }}</td>
                        <td class="text-end px-0">{{
                                getFormattedDate(closedInvoice.creationTime)
                            }}
                        </td>
                        <td class="text-end px-0">{{ closedInvoice.finalTotalPrice }}</td>
                        <td class="text-end px-0">
                            <span
                                v-if="closedInvoice.order!.orderAcceptanceStatus === OrderAcceptanceStatusEnum.GivenToClient && closedInvoice.paymentCompleted"
                                class="text-success">Yes</span>
                            <span v-else class="text-danger">No (Cancelled)</span>
                        </td>

                    </tr>
                    </tbody>
                </table>
                <div v-else>
                    Sorry there are not any open invoicesdsds
                </div>
            </div>
        </div>
        <div></div>

    </div>
</template>

<script lang="ts" setup>

import {useIdentityStore} from "@/stores/identityStore";
import {useRouter} from "vue-router";
import {onBeforeMount, ref} from "vue";
import InvoicesService from "@/services/shop/InvoicesService";
import type {IInvoice} from "../../dto/shop/IInvoice";
import {getFormattedDate} from "@/helpers/UnifiedFormatter";
import {OrderAcceptanceStatusEnum} from "@/dto/enums/OrderAcceptanceStatusEnum";
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";
import {InvoiceAcceptanceStatusEnum} from "@/dto/enums/InvoiceAcceptanceStatusEnum";

const identitySore = useIdentityStore();

const invoicesService = new InvoicesService();


const router = useRouter()

const openInvoices = ref<IInvoice[]>([]);
const closedInvoices = ref<IInvoice[]>([]);

onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();

    let identity = identitySore.authenticationJwt;

    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }

    let invoices = await invoicesService.getAllUserInvoices(identity);

    if (invoices) {
        invoices.forEach(function (item) {
            if (item.invoiceAcceptanceStatus === InvoiceAcceptanceStatusEnum.BusinessRejected || (item.order!.orderAcceptanceStatus === OrderAcceptanceStatusEnum.GivenToClient && item.paymentCompleted)) {
                closedInvoices.value.push(item)
            } else {
                openInvoices.value.push(item)
            }
        });
    }
})

const openInvoiceOrderDetails = async (invoiceId: string) => {
    console.log('openInvoiceOrderDetails', invoiceId);
    await router.push({name: 'invoiceOrder', params: {id: invoiceId}})
}


</script>


<style scoped>
.custom-link {
    cursor: pointer;
    text-decoration-line: underline;
}
</style>