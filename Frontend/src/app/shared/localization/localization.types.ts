// Empty localization types file
import { LOCALIZATION_KEYS } from './localization.constants';

export type LocalizationKey = (typeof LOCALIZATION_KEYS)[keyof typeof LOCALIZATION_KEYS];
