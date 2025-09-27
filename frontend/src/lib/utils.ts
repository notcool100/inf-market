export function formatCurrency(amount: number, currency: string = 'NPR'): string {
  return new Intl.NumberFormat('en-NP', {
    style: 'currency',
    currency: currency,
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  }).format(amount);
}

export function formatDate(date: string | Date): string {
  const d = typeof date === 'string' ? new Date(date) : date;
  return d.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
  });
}

export function formatDateTime(date: string | Date): string {
  const d = typeof date === 'string' ? new Date(date) : date;
  return d.toLocaleString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  });
}

export function truncateText(text: string, maxLength: number): string {
  if (text.length <= maxLength) return text;
  return text.substring(0, maxLength) + '...';
}

export function getStatusColor(status: string): string {
  const statusColors: Record<string, string> = {
    'Draft': 'bg-gray-100 text-gray-800',
    'Open': 'bg-blue-100 text-blue-800',
    'InProgress': 'bg-yellow-100 text-yellow-800',
    'PendingReview': 'bg-orange-100 text-orange-800',
    'Completed': 'bg-green-100 text-green-800',
    'Cancelled': 'bg-red-100 text-red-800',
    'Disputed': 'bg-purple-100 text-purple-800',
    'Pending': 'bg-yellow-100 text-yellow-800',
    'InEscrow': 'bg-blue-100 text-blue-800',
    'Released': 'bg-green-100 text-green-800',
    'Refunded': 'bg-red-100 text-red-800',
    'Failed': 'bg-red-100 text-red-800',
  };
  
  return statusColors[status] || 'bg-gray-100 text-gray-800';
}

export function debounce<T extends (...args: any[]) => any>(
  func: T,
  wait: number
): (...args: Parameters<T>) => void {
  let timeout: NodeJS.Timeout;
  return (...args: Parameters<T>) => {
    clearTimeout(timeout);
    timeout = setTimeout(() => func(...args), wait);
  };
}