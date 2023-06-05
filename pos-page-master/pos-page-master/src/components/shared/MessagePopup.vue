<template>
    <div :class="'alert ' + popupMessage.type + ' alert-dismissible fade show  mt-alerts' " role="alert">

        <div class="row">
            <div class="col-md-12">

                <div class="col-md-11">
                    <strong>({{ props.count }}) <span v-if="props.popupMessage.status"> {{
                            props.popupMessage.status
                        }}</span></strong>
                    <p>
                        {{ props.popupMessage.message }}
                    </p>
                </div>
                <div class="col-md-1">
                    <button type="button" class="btn-close" aria-label="Close" @click="sendRemovalEmit"></button>
                </div>

            </div>
        </div>

    </div>
</template>


<script lang="ts" setup>

import type {IMessage} from "@/dto/shared/IMessage";
import {onMounted, onUnmounted} from "vue";

interface IProps {
    popupMessage: IMessage,
    count: number
}

const emits = defineEmits(['handleMessageRemoval']);
const props = defineProps<IProps>()

// Emit the child event with a message
const sendRemovalEmit = () => {
    emits('handleMessageRemoval', props.popupMessage);
};
let timerId: number;


onMounted(() => {

    timerId = setInterval(async () => {
        sendRemovalEmit()
    }, 15000);
});

onUnmounted(() => {
    clearInterval(timerId);
});
</script>

<style scoped>
.mt-alerts {
    position: fixed;
    left: 25%;
    right: 25%;
    z-index: 9999;
}

</style>