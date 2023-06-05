<template>
    <section>
        <!--Grid row-->
        <div class="row mb-2">
            <!--Grid column-->
            <div class="col-md-6">

                <img v-if="!props.businessDetails.picturePath || props.businessDetails.picturePath <= 0"
                     src="https://images.unsplash.com/photo-1528698827591-e19ccd7bc23d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1176&q=80"
                     class="img-fluid" alt=""/>
                <img v-else
                     :src="props.businessDetails.picturePath"
                     class="img-fluid" alt=""/>
            </div>
            <!--Grid column-->

            <!--Grid column-->
            <div class="col-md-6">
                <!--Content-->
                <div class="p-4">
                    <p style="font-size: 20px;"><strong>{{ props.businessDetails.name }}</strong></p>

                    <div class="" v-if="props.businessDetails.businessCategory">
                        <span class="badge bg-info me-1">{{ props.businessDetails.businessCategory.title }}</span>
                    </div>


                    <p>{{
                            props. businessDetails.description
                        }}</p>

                    <SimpleRowValuePair
                        strongText="Address: "
                        :normalText="props.businessDetails.address"
                    />
                    <SimpleRowValuePair
                        strongText="Email: "
                        :normalText="props.businessDetails.email"
                    />
                    <SimpleRowValuePair
                        strongText="Phone number: "
                        :normalText="props.businessDetails.phoneNumber"
                    />

                    <slot>
                    </slot>
                </div>
                <!--Content-->

            </div>
            <!--Grid column-->
        </div>

        <!--Grid row-->
    </section>
</template>
<script setup lang="ts">
import type {IBusiness} from "@/dto/shop/IBusiness";
import SimpleRowValuePair from "@/components/Shops/Elements/SimpleRowValuePair.vue";
import type {IManagerBusiness} from "@/dto/manager/IManagerBusiness";

interface IProps {
    businessDetails: IBusiness | IManagerBusiness,
}

const props = defineProps<IProps>();

// watch(() => [props.progressbarWidth, props.strongText, props.header], () => {});


</script>