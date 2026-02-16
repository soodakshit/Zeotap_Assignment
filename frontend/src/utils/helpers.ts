import { Severity, Status } from '../types/incident';

export function formatDate(dateString: string): string {
  const date = new Date(dateString);
  return new Intl.DateTimeFormat('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit',
  }).format(date);
}

export function getSeverityColor(severity: Severity): string {
  switch (severity) {
    case Severity.SEV1:
      return 'bg-red-100 text-red-800 border-red-200';
    case Severity.SEV2:
      return 'bg-orange-100 text-orange-800 border-orange-200';
    case Severity.SEV3:
      return 'bg-yellow-100 text-yellow-800 border-yellow-200';
    case Severity.SEV4:
      return 'bg-blue-100 text-blue-800 border-blue-200';
    default:
      return 'bg-gray-100 text-gray-800 border-gray-200';
  }
}

export function getStatusColor(status: Status): string {
  switch (status) {
    case Status.OPEN:
      return 'bg-red-100 text-red-800 border-red-200';
    case Status.MITIGATED:
      return 'bg-yellow-100 text-yellow-800 border-yellow-200';
    case Status.RESOLVED:
      return 'bg-green-100 text-green-800 border-green-200';
    default:
      return 'bg-gray-100 text-gray-800 border-gray-200';
  }
}

export function debounce<T extends (...args: any[]) => any>(
  func: T,
  delay: number
): (...args: Parameters<T>) => void {
  let timeoutId: ReturnType<typeof setTimeout>;
  return (...args: Parameters<T>) => {
    clearTimeout(timeoutId);
    timeoutId = setTimeout(() => func(...args), delay);
  };
}