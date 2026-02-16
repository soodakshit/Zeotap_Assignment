import axios from 'axios';
import {
  Incident,
  CreateIncidentRequest,
  UpdateIncidentRequest,
  PagedResponse,
  IncidentQueryParams,
} from '../types/incident';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export const incidentService = {
  async getIncidents(params: IncidentQueryParams): Promise<PagedResponse<Incident>> {
    const response = await api.get<PagedResponse<Incident>>('/incidents', { params });
    return response.data;
  },

  async getIncident(id: string): Promise<Incident> {
    const response = await api.get<Incident>(`/incidents/${id}`);
    return response.data;
  },

  async createIncident(data: CreateIncidentRequest): Promise<Incident> {
    const response = await api.post<Incident>('/incidents', data);
    return response.data;
  },

  async updateIncident(id: string, data: UpdateIncidentRequest): Promise<Incident> {
    const response = await api.patch<Incident>(`/incidents/${id}`, data);
    return response.data;
  },
};
