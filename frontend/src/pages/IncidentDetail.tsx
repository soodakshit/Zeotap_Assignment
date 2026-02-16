import { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { incidentService } from '../services/incidentService';
import { Incident, Severity, Status, UpdateIncidentRequest } from '../types/incident';
import { formatDate, getSeverityColor, getStatusColor } from '../utils/helpers';

export default function IncidentDetail() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  
  const [incident, setIncident] = useState<Incident | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [editing, setEditing] = useState(false);
  const [saving, setSaving] = useState(false);
  
  const [formData, setFormData] = useState({
    title: '',
    service: '',
    severity: Severity.SEV3,
    status: Status.OPEN,
    owner: '',
    summary: '',
  });

  useEffect(() => {
    if (id) {
      fetchIncident();
    }
  }, [id]);

  const fetchIncident = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await incidentService.getIncident(id!);
      setIncident(data);
      setFormData({
        title: data.title,
        service: data.service,
        severity: data.severity,
        status: data.status,
        owner: data.owner || '',
        summary: data.summary || '',
      });
    } catch (err: any) {
      setError(err.message || 'Failed to fetch incident');
    } finally {
      setLoading(false);
    }
  };

  const handleSave = async () => {
    try {
      setSaving(true);
      setError(null);
      
      const updateData: UpdateIncidentRequest = {};
      if (formData.title !== incident?.title) updateData.title = formData.title;
      if (formData.service !== incident?.service) updateData.service = formData.service;
      if (formData.severity !== incident?.severity) updateData.severity = formData.severity;
      if (formData.status !== incident?.status) updateData.status = formData.status;
      if (formData.owner !== (incident?.owner || '')) updateData.owner = formData.owner || '';
      if (formData.summary !== (incident?.summary || '')) updateData.summary = formData.summary || '';
      
      const updated = await incidentService.updateIncident(id!, updateData);
      setIncident(updated);
      setEditing(false);
    } catch (err: any) {
      setError(err.message || 'Failed to update incident');
    } finally {
      setSaving(false);
    }
  };

  const handleCancel = () => {
    if (incident) {
      setFormData({
        title: incident.title,
        service: incident.service,
        severity: incident.severity,
        status: incident.status,
        owner: incident.owner || '',
        summary: incident.summary || '',
      });
    }
    setEditing(false);
    setError(null);
  };

  if (loading) {
    return (
      <div className="max-w-4xl mx-auto px-4 py-8">
        <div className="flex justify-center items-center h-64">
          <div className="inline-block animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
        </div>
      </div>
    );
  }

  if (error && !incident) {
    return (
      <div className="max-w-4xl mx-auto px-4 py-8">
        <div className="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded">
          Error: {error}
        </div>
        <button
          onClick={() => navigate('/incidents')}
          className="mt-4 text-blue-600 hover:text-blue-800"
        >
          ← Back to incidents
        </button>
      </div>
    );
  }

  if (!incident) {
    return null;
  }

  return (
    <div className="max-w-4xl mx-auto px-4 py-8">
      <div className="mb-6">
        <button
          onClick={() => navigate('/incidents')}
          className="text-blue-600 hover:text-blue-800 mb-4"
        >
          ← Back to incidents
        </button>
        <div className="flex justify-between items-start">
          <div>
            <h1 className="text-3xl font-bold text-gray-900 mb-2">Incident Details</h1>
            <p className="text-sm text-gray-500">ID: {incident.id}</p>
          </div>
          {!editing ? (
            <button
              onClick={() => setEditing(true)}
              className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md font-medium transition-colors"
            >
              Edit
            </button>
          ) : (
            <div className="space-x-2">
              <button
                onClick={handleSave}
                disabled={saving}
                className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-md font-medium transition-colors disabled:opacity-50"
              >
                {saving ? 'Saving...' : 'Save'}
              </button>
              <button
                onClick={handleCancel}
                disabled={saving}
                className="bg-gray-200 hover:bg-gray-300 text-gray-800 px-4 py-2 rounded-md font-medium transition-colors disabled:opacity-50"
              >
                Cancel
              </button>
            </div>
          )}
        </div>
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-800 px-4 py-3 rounded mb-6">
          {error}
        </div>
      )}

      <div className="bg-white rounded-lg shadow-md p-6 space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Title
          </label>
          {editing ? (
            <input
              type="text"
              value={formData.title}
              onChange={(e) => setFormData({ ...formData, title: e.target.value })}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          ) : (
            <p className="text-lg text-gray-900">{incident.title}</p>
          )}
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Service
            </label>
            {editing ? (
              <input
                type="text"
                value={formData.service}
                onChange={(e) => setFormData({ ...formData, service: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              />
            ) : (
              <p className="text-gray-900">{incident.service}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Owner
            </label>
            {editing ? (
              <input
                type="text"
                value={formData.owner}
                onChange={(e) => setFormData({ ...formData, owner: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Optional"
              />
            ) : (
              <p className="text-gray-900">{incident.owner || '-'}</p>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Severity
            </label>
            {editing ? (
              <select
                value={formData.severity}
                onChange={(e) => setFormData({ ...formData, severity: e.target.value as Severity })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="SEV1">SEV1</option>
                <option value="SEV2">SEV2</option>
                <option value="SEV3">SEV3</option>
                <option value="SEV4">SEV4</option>
              </select>
            ) : (
              <span className={`px-3 py-1 inline-flex text-sm font-semibold rounded-full border ${getSeverityColor(incident.severity)}`}>
                {incident.severity}
              </span>
            )}
          </div>

          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Status
            </label>
            {editing ? (
              <select
                value={formData.status}
                onChange={(e) => setFormData({ ...formData, status: e.target.value as Status })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              >
                <option value="OPEN">Open</option>
                <option value="MITIGATED">Mitigated</option>
                <option value="RESOLVED">Resolved</option>
              </select>
            ) : (
              <span className={`px-3 py-1 inline-flex text-sm font-semibold rounded-full border ${getStatusColor(incident.status)}`}>
                {incident.status}
              </span>
            )}
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Summary
          </label>
          {editing ? (
            <textarea
              value={formData.summary}
              onChange={(e) => setFormData({ ...formData, summary: e.target.value })}
              rows={4}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
              placeholder="Optional summary of the incident..."
            />
          ) : (
            <p className="text-gray-900 whitespace-pre-wrap">
              {incident.summary || '-'}
            </p>
          )}
        </div>

        <div className="pt-4 border-t border-gray-200">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 text-sm">
            <div>
              <span className="font-medium text-gray-700">Created:</span>{' '}
              <span className="text-gray-900">{formatDate(incident.createdAt)}</span>
            </div>
            <div>
              <span className="font-medium text-gray-700">Updated:</span>{' '}
              <span className="text-gray-900">{formatDate(incident.updatedAt)}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
