<template>

    <section class="py-5 text-center container">
        <div class="row py-lg-5">
            <div class="col-lg-6 col-md-8 mx-auto">
                <p>
                    <RouterLink to="/store/business">Start your journey</RouterLink>
                </p>
            </div>
        </div>
        <!--
                <div>
                    <p>Input</p>
                    <input type="file" accept="image/*" @change="handleFileInputChange">
                    <button @click="uploadImage" :disabled="!selectedImage">Upload</button>
                    <div v-if="uploadStatus">{{ uploadStatus }}</div>
                </div>
                <div>
                    <button type="button" class="btn btn-danger" @click="deleteThumbnails">Danger delete all images</button>
                </div>
                <div>
                    <h1>Uploaded images</h1>
                    <div v-if="uploadedImages">

                        <p>Here are [{{ uploadedImages.length }}]</p>

                        <div class="card" v-for="image in uploadedImages" :key="image">
                            <img :src="image" class="card-img-top" alt="...">
                        </div>
                    </div>
                </div>
                <div>
                    <p>Images not found</p>
                </div>

                -->
    </section>
</template>

<script setup lang='ts'>
import {onBeforeMount} from 'vue';
import {RouterLink} from 'vue-router';

onBeforeMount(async () => {
});



</script>

