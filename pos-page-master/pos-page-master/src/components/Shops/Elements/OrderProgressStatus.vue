<template>
  <div>
    <div class="progress" role="progressbar" aria-label="Success example" :aria-valuenow="props.progressbarWidth" aria-valuemin="0"
         aria-valuemax="101">
      <div class="progress-bar bg-success"
           :style="{width: props.progressbarWidth + '%'}">
      </div>
    </div>
    <h4 class="text-center font-weight-bold pt-4 pb-5">
      <strong>
        {{ header }}
      </strong>
    </h4>
    <p class="text-center" v-if="strongText && strongText.trim().length > 0">
      <strong>
        {{ strongText }}
      </strong>
    </p>
  </div>

</template>
<script setup lang="ts">

const props = defineProps({
  header: String,
  strongText: String,
  progressbarWidth: String
});

// watch(() => [props.progressbarWidth, props.strongText, props.header], () => {});


</script>