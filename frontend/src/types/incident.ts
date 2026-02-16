export enum Severity {
  SEV1 = 'SEV1',
  SEV2 = 'SEV2',
  SEV3 = 'SEV3',
  SEV4 = 'SEV4',
}

export enum Status {
  OPEN = 'OPEN',
  MITIGATED = 'MITIGATED',
  RESOLVED = 'RESOLVED',
}

export interface Incident {
  id: string;
  title: string;
  service: string;
  severity: Severity;
  status: Status;
  owner: string | null;
  summary: string | null;
  createdAt: string;
  updatedAt: string;
}

export interface CreateIncidentRequest {
  title: string;
  service: string;
  severity: Severity;
  status: Status;
  owner?: string;
  summary?: string;
}

export interface UpdateIncidentRequest {
  title?: string;
  service?: string;
  severity?: Severity;
  status?: Status;
  owner?: string;
  summary?: string;
}

export interface PagedResponse<T> {
  data: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface IncidentQueryParams {
  page?: number;
  pageSize?: number;
  search?: string;
  service?: string;
  severity?: Severity;
  status?: Status;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}
