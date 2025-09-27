<script lang="ts">
  import { getStatusColor } from '../utils';
  
  export let status: string;
  export let size: 'sm' | 'md' = 'sm';
  
  $: colorClass = getStatusColor(status);
  $: sizeClass = size === 'sm' ? 'px-2 py-1 text-xs' : 'px-3 py-1 text-sm';
</script>

<span class="inline-flex items-center {sizeClass} font-medium rounded-full {colorClass}">
  {status}
</span>