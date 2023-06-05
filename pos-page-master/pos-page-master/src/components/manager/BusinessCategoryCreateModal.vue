<template>
    <!-- Start modal-->
    <section>
        <div class="d-flex">

            <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal"
                    :data-bs-target="'#' + uniqueId">
                Add new business category
            </button>
            <!-- Modal -->
            <div class="modal fade" :id="uniqueId" data-bs-backdrop="static" data-bs-keyboard="false"
                 tabindex="-1" :aria-labelledby="uniqueId + 'Label'" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h1 class="modal-title fs-5" :id="uniqueId + 'Label'">
                                Add new business category
                            </h1>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"
                                    aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-6 mb-3">
                                    <label>Business category</label>
                                    <input type="text" class="form-control"
                                           v-model="displayData.title">
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close
                            </button>
                            <button type="button" class="btn btn-primary" v-on:click="onSubmit">
                                Add new business category
                            </button>
                            <p :id="uniqueHiderId" data-bs-dismiss="modal" style="visibility: hidden"></p>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- End modal-->
</template>

<script lang="ts" setup>


import {onBeforeMount, ref} from "vue";
import {useIdentityStore} from "@/stores/identityStore";
import {generateRandomString} from "@/helpers/Randomiser";
import type {IManagerBusinessCategory} from "@/dto/manager/IManagerBusinessCategory";
import {BusinessCategoriesService} from "@/services/manager/BusinessCategoriesService";

const identitySore = useIdentityStore();
const businessCategoryService = new BusinessCategoriesService();
const uniqueId = ref<string>('s' + generateRandomString())
const uniqueHiderId = ref<string>('s' + generateRandomString())


// Define the props and emits
const emits = defineEmits(['update']);

// Create a localData ref to hold the updated values
const displayData = ref<IManagerBusinessCategory>({
    title: ""
} as IManagerBusinessCategory)

onBeforeMount(async () => {
})





const onSubmit = async (event: MouseEvent) => {
    event.preventDefault();

    let identity = identitySore.authenticationJwt;

    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }
    let businessCategory: IManagerBusinessCategory | undefined;

    if (displayData.value) {
        businessCategory = await businessCategoryService.create(identity, displayData.value!)
    }
    if (businessCategory) {
        console.log("Business category was  was successfully created")
    } else {
        console.error("Unable to create Business category")
        return
    }
    emits('update');
    let hider = document.getElementById(uniqueHiderId.value);
    hider!.click()
}


</script>