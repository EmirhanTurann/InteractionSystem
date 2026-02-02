# GAMEDEV_INTERCASE – Prompt & İnceleme Rehberi

Bu dosya, World Interaction System case’i incelerken **hangi bakış açısıyla** değerlendirme yapılması gerektiğini ve
sistemin **nasıl test edilmesinin önerildiğini** açıklar.

Amaç; UI veya görsel sunumdan ziyade **mimari kararları ve sistem kalitesini** görünür kılmaktır.

---

## İnceleme Yaklaşımı

Bu case değerlendirilirken aşağıdaki sorulara odaklanılması beklenir:

- Sistem **genişletilebilir mi?**
- Interactor ile Interactable arasındaki bağımlılık minimum mu?
- Yeni bir etkileşim türü eklemek için mevcut kodu değiştirmek gerekiyor mu?
- Etkileşim kuralları (lock, key, hold vs.) doğru katmanda mı çözülmüş?

---

## Sistem Kapsamı

Bu case’de bilinçli olarak aşağıdaki öncelikler belirlenmiştir:

- UI/UX detayları **öncelik dışıdır**
- Odak noktası:
  - Interaction contracts
  - Interaction flows
  - Context tabanlı karar mekanizmalarıdır

> Yarım bırakılmış çok sayıda özellik yerine, tamamlanmış ve tutarlı bir çekirdek sistem hedeflenmiştir.

---

## Mevcut Etkileşim Türleri

Sistem aşağıdaki etkileşim türlerini destekler:

### Instant
- Anında gerçekleşen etkileşimler
- Örnek: Anahtar toplama (KeyInteractable)

### Hold
- Belirli bir süre basılı tutulması gereken etkileşimler
- Örnek: Sandık açma (ChestInteractable)

### Toggle
- Aç/Kapa mantığıyla çalışan etkileşimler
- Örnek: Kapı, Switch, Light

---

## Örnek Interactable’lar

Sistemin davranışlarını göstermek için şu örnekler eklenmiştir:

- **DoorInteractable**
  - Anahtar ile kilit açma
  - Toggle tabanlı çalışma

- **ChestInteractable**
  - Hold tabanlı
  - Kilit veya anahtar içermez (bilinçli tasarım kararı)

- **KeyInteractable**
  - Instant etkileşim örneği
  - Envanter entegrasyonu

- **SwitchInteractable**
  - Toggle etkileşimi
  - Hedefleri interface üzerinden tetikler

- **PointLightTarg**
