<template>
    <!-- Start modal-->
    <section>
        <div class="d-flex" v-if="props.invoiceData">
            <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal"
                    :data-bs-target="'#' + uniqueId">
                {{ invoiceData.id }}
            </button>
            <!-- Modal -->
            <div class="modal fade" :id="uniqueId" data-bs-backdrop="static" data-bs-keyboard="false"
                 tabindex="-1" :aria-labelledby="uniqueId + 'Label'" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-body">
                            <InvoiceDetailsCard :invoice-data-val="props.invoiceData"/>
                            <button type="button" class="btn btn-primary w-100 mt-1"  :id="uniqueHiderId" data-bs-dismiss="modal" >
                                Close
                            </button>
                            <slot></slot>
                        </div>

                    </div>
                </div>

            </div>
        </div>
        <div v-else>
            {{props.invoiceData}}
            Error with invoice data
        </div>
    </section>
    <!-- End modal-->
</template>

<script lang="ts" setup>


import { ref} from "vue";
import {generateRandomString} from "@/helpers/Randomiser";
import type {IManagerInvoice} from "@/dto/manager/IManagerInvoice";
import InvoiceDetailsCard from "@/components/Shops/InvoiceDetailsCard.vue";
import type {IInvoice} from "@/dto/shop/IInvoice";


interface IProps {
    invoiceData: IManagerInvoice | IInvoice |undefined,
}

const uniqueId = ref<string>(generateRandomString())
const uniqueHiderId = ref<string>(generateRandomString())


// Define the props and emits
const props = defineProps<IProps>();
</script>