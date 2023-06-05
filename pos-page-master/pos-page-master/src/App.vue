<template>
    <NavBar/>
    <body>

    <div v-if="messageIsFound" >
        <MessagePopup v-for="(errorMessage, index) in messageStore.getAllMessages()"
                      :key="index"
                      :popupMessage="errorMessage"
                      :count="index + 1"
                      @handleMessageRemoval="handleRemoval"/>
    </div>
    <div class="container">
        <main role="main" class="pb-3">
            <router-view></router-view>
        </main>
    </div>
    </body>
    <FooterBar/>
</template>

<script setup lang="ts">
import {RouterView} from 'vue-router';
import NavBar from '@/components/NavBar.vue';
import FooterBar from "@/components/FooterBar.vue";
import {useMessageStore} from "@/stores/messageStore";
import {onBeforeMount,  onUpdated} from "vue";
import MessagePopup from "@/components/shared/MessagePopup.vue";
import {computed} from 'vue';
import type {IMessage} from "@/dto/shared/IMessage";

const messageStore = useMessageStore();



// Compute a reactive variable to determine if the popup should be shown
const messageIsFound = computed(() => {
    return messageStore.messageExists();
});


onBeforeMount(() => {


})

const handleRemoval = (message: IMessage) => {
    messageStore.removeSpecifiedMessage(message);
};


onUpdated(() => {
})
</script>
<style scoped></style>


