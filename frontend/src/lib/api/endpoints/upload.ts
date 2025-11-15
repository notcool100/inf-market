/**
 * File Upload API Endpoints
 */

import apiClient from '../client';
import type { FileUploadResult } from '../types';

export const uploadApi = {
  /**
   * Upload image file
   */
  uploadImage: async (
    file: File,
    onProgress?: (progress: number) => void
  ): Promise<FileUploadResult> => {
    return apiClient.uploadFile('/api/upload/image', file, onProgress);
  },

  /**
   * Upload document file
   */
  uploadDocument: async (
    file: File,
    onProgress?: (progress: number) => void
  ): Promise<FileUploadResult> => {
    return apiClient.uploadFile('/api/upload/document', file, onProgress);
  },

  /**
   * Delete file
   */
  deleteFile: async (filename: string): Promise<void> => {
    return apiClient.delete(`/api/upload/${filename}`);
  },
};

