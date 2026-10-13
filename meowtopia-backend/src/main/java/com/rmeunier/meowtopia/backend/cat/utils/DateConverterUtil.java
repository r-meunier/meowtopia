package com.rmeunier.meowtopia.backend.cat.utils;

import java.time.Instant;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.ZoneId;
import java.util.Date;

public class DateConverterUtil {

    public static LocalDateTime dateToLocalDateTime(Date dateToConvert) {
        return Instant.ofEpochMilli(dateToConvert.getTime())
                .atZone(ZoneId.systemDefault())
                .toLocalDateTime();
    }

    public static Date localDateTimeToDate(LocalDateTime dateToConvert) {
        return Date.from(dateToConvert.atZone(ZoneId.systemDefault()).toInstant());
    }

    public static LocalDate dateToLocalDate(Date dateToConvert) {
        return Instant.ofEpochMilli(dateToConvert.getTime())
                .atZone(ZoneId.systemDefault())
                .toLocalDate();
    }

    public static Date localDateToDate(LocalDate dateToConvert) {
        return Date.from(dateToConvert.atStartOfDay(ZoneId.systemDefault()).toInstant());
    }
}
