<template>
    <div class="card">
        <div class="card-body p-5" v-if="invoiceDataVal">
            <h2>
                Hey ,
            </h2>
            <p class="fs-sm">
                This is the invoice <strong>{{props.invoiceDataVal.id}}</strong> for a payment of <strong>{{ props.invoiceDataVal.finalTotalPrice }}</strong>
            </p>

            <table class="table border-bottom border-gray-200 mt-3">
                <thead>
                <tr>
                    <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm px-0">Product Name</th>
                    <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm px-0">Product Count</th>
                    <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm px-0">Unit Price</th>
                    <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Tax Percent</th>
                    <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Tax Amount</th>
                    <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm text-end px-0">Amount</th>
                </tr>
                </thead>
                <tbody v-if="invoiceDataVal.invoiceRows">
                <tr v-for="invoiceRow in invoiceDataVal.invoiceRows" :key="invoiceRow.id">
                    <td class="px-0">{{ invoiceRow.productName }}</td>
                    <td class="px-0">{{ invoiceRow.productUnitCount }}</td>
                    <td class="px-0">{{ invoiceRow.productPricePerUnit }}</td>
                    <td class="text-end px-0">{{ invoiceRow.taxPercent }}</td>
                    <td class="text-end px-0">{{ invoiceRow.taxAmountFromPercent }}</td>
                    <td class="text-end px-0">{{ invoiceRow.finalProductPrice }}</td>
                </tr>
                </tbody>
                <tbody v-else>
                Unable to load table data
                </tbody>
            </table>

            <div class="mt-5">
                <div class="d-flex justify-content-end">
                    <p class="text-muted me-3">Subtotal:</p>
                    <span>{{ invoiceDataVal.totalPriceWithoutTax }}</span>
                </div>

                <div class="d-flex justify-content-end">
                    <p class="text-muted me-3">Tax:</p>
                    <span>{{ invoiceDataVal.taxAmount }}</span>
                </div>

                <div class="d-flex justify-content-end mt-3">
                    <h5 class="m<

                !---->e-3">Total:</h5>
                    <h5 class="text-success">{{ invoiceDataVal.finalTotalPrice }}</h5>
                </div>
            </div>
        </div>
        <div v-else>Faulty invoice data was received</div>
        <slot></slot>
    </div>
</template>


<script lang="ts" setup>
import type {IInvoice} from "@/dto/shop/IInvoice";
import type {IManagerInvoice} from "@/dto/manager/IManagerInvoice";

interface IProps {
    invoiceDataVal: IInvoice | IManagerInvoice,
}

const props = defineProps<IProps>();


</script>


