# \# World Interaction System

# 

# Bu proje, UI/UX detaylarından ziyade \*\*temiz, genişletilebilir ve modüler bir etkileşim sistemi\*\* tasarımını göstermeyi amaçlar.

# 

# Case kapsamında odak noktası:

# \- Etkileşim mimarisi

# \- Sorumlulukların doğru ayrılması

# \- Genişletilebilirliktir

# 

# ---

# 

# \## Genel Bakış

# 

# Sistem; küçük ama net tanımlanmış \*\*çekirdek kontratlar\*\* ve \*\*etkileşim akışları\*\* üzerine kuruludur.  

# Interactor ile Interactable birbirini \*\*doğrudan tanımaz\*\*; tüm iletişim arayüzler üzerinden gerçekleşir.

# 

# ---

# 

# \## Çekirdek Mimari

# 

# \### Core Kontratlar

# `Runtime/Core` altında bulunur:

# 

# \- `IInteractable`  

# \- `InteractionType`  

# \- `InteractorContext`

# 

# Bu yapılar, sahnedeki herhangi bir nesnenin etkileşilebilir olabilmesi için gereken minimum sözleşmeyi tanımlar.

# 

# \### Etkileşim Türleri

# Temel etkileşim akışları base class’lar ile sağlanır:

# 

# \- \*\*InstantInteractable\*\*  

# &nbsp; Anında gerçekleşen etkileşimler için kullanılır (ör. anahtar alma).

# 

# \- \*\*HoldInteractable\*\*  

# &nbsp; Belirli bir süre basılı tutmayı gerektiren etkileşimler için kullanılır  

# &nbsp; (ör. sandık açma).

# 

# \- \*\*ToggleInteractable\*\*  

# &nbsp; Aç/Kapa gibi iki durumlu etkileşimler için kullanılır  

# &nbsp; (ör. kapı, switch).

# 

# ---

# 

# \## Interactor

# 

# \### SphereInteractor

# `SphereInteractor`, etkileşimi başlatan taraftır ve şu sorumluluklara sahiptir:

# 

# \- Yakındaki etkileşilebilir nesneleri taramak

# \- Opsiyonel olarak görüş açısı ve line-of-sight filtreleri uygulamak

# \- Mesafe ve açıya göre adayları puanlamak

# \- En uygun hedefi seçmek

# \- Hold etkileşimlerinde süre/progress yönetimini yapmak

# 

# Interactor, \*\*hiçbir concrete interactable tipini bilmez\*\*.  

# Tüm kararlar interface ve interaction check mekanizması üzerinden verilir.

# 

# ---

# 

# \## Örnek Etkileşilebilir Nesneler

# 

# Sistemin nasıl kullanılacağını göstermek için aşağıdaki örnekler eklenmiştir:

# 

# \### DoorInteractable

# \- Toggle tabanlıdır

# \- Her kapı için ayrı anahtar tanımlanabilir

# \- Kilitliyken etkileşim engellenir

# \- İstenirse anahtar kilit açıldığında tüketilebilir

# 

# \### ChestInteractable

# \- Hold tabanlıdır

# \- Belirlenen süre tamamlandığında açılır

# \- Bilinçli olarak kilit veya anahtar mantığı içermez

# 

# \### KeyInteractable

# \- Instant etkileşim örneğidir

# \- Etkileşim sırasında anahtarı oyuncunun envanterine ekler

# \- Instant interaction akışını minimal şekilde gösterir

# 

# \### SwitchInteractable

# \- Toggle tabanlıdır

# \- Bir veya birden fazla hedefe sinyal gönderebilir

# \- Hedeflerin nasıl tepki vereceğini bilmez

# 

# \### PointLightTarget

# \- Switch için örnek bir hedef implementasyonudur

# \- `ISwitchTarget` arayüzünü uygular

# \- Switch etkileşimini görsel olarak doğrular

# 

# ---

# 

# \## Switch Target Sistemi

# 

# Switch ile hedefler arasındaki bağlantı, basit bir arayüz ile ayrıştırılmıştır:

# 

# ```csharp

# public interface ISwitchTarget

# {

# &nbsp;   void SetActive(bool isActive, InteractorContext context);

# }



