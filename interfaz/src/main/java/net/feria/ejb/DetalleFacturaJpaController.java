/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.ejb;

import java.io.Serializable;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Query;
import javax.persistence.EntityNotFoundException;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import net.feria.ejb.exceptions.NonexistentEntityException;
import net.feria.entity.DetalleFactura;
import net.feria.entity.FacturasCabecera;
import net.feria.entity.Item;

/**
 *
 * @author Deth1
 */
public class DetalleFacturaJpaController implements Serializable {

    public DetalleFacturaJpaController(EntityManagerFactory emf) {
        this.emf = emf;
    }
    private EntityManagerFactory emf = null;

    public EntityManager getEntityManager() {
        return emf.createEntityManager();
    }

    public void create(DetalleFactura detalleFactura) {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            FacturasCabecera codigoFactura = detalleFactura.getCodigoFactura();
            if (codigoFactura != null) {
                codigoFactura = em.getReference(codigoFactura.getClass(), codigoFactura.getCodigoFactura());
                detalleFactura.setCodigoFactura(codigoFactura);
            }
            Item codigoItem = detalleFactura.getCodigoItem();
            if (codigoItem != null) {
                codigoItem = em.getReference(codigoItem.getClass(), codigoItem.getCodigoItem());
                detalleFactura.setCodigoItem(codigoItem);
            }
            em.persist(detalleFactura);
            if (codigoFactura != null) {
                codigoFactura.getDetalleFacturaCollection().add(detalleFactura);
                codigoFactura = em.merge(codigoFactura);
            }
            if (codigoItem != null) {
                codigoItem.getDetalleFacturaCollection().add(detalleFactura);
                codigoItem = em.merge(codigoItem);
            }
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void edit(DetalleFactura detalleFactura) throws NonexistentEntityException, Exception {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            DetalleFactura persistentDetalleFactura = em.find(DetalleFactura.class, detalleFactura.getCodigoDetalleFactura());
            FacturasCabecera codigoFacturaOld = persistentDetalleFactura.getCodigoFactura();
            FacturasCabecera codigoFacturaNew = detalleFactura.getCodigoFactura();
            Item codigoItemOld = persistentDetalleFactura.getCodigoItem();
            Item codigoItemNew = detalleFactura.getCodigoItem();
            if (codigoFacturaNew != null) {
                codigoFacturaNew = em.getReference(codigoFacturaNew.getClass(), codigoFacturaNew.getCodigoFactura());
                detalleFactura.setCodigoFactura(codigoFacturaNew);
            }
            if (codigoItemNew != null) {
                codigoItemNew = em.getReference(codigoItemNew.getClass(), codigoItemNew.getCodigoItem());
                detalleFactura.setCodigoItem(codigoItemNew);
            }
            detalleFactura = em.merge(detalleFactura);
            if (codigoFacturaOld != null && !codigoFacturaOld.equals(codigoFacturaNew)) {
                codigoFacturaOld.getDetalleFacturaCollection().remove(detalleFactura);
                codigoFacturaOld = em.merge(codigoFacturaOld);
            }
            if (codigoFacturaNew != null && !codigoFacturaNew.equals(codigoFacturaOld)) {
                codigoFacturaNew.getDetalleFacturaCollection().add(detalleFactura);
                codigoFacturaNew = em.merge(codigoFacturaNew);
            }
            if (codigoItemOld != null && !codigoItemOld.equals(codigoItemNew)) {
                codigoItemOld.getDetalleFacturaCollection().remove(detalleFactura);
                codigoItemOld = em.merge(codigoItemOld);
            }
            if (codigoItemNew != null && !codigoItemNew.equals(codigoItemOld)) {
                codigoItemNew.getDetalleFacturaCollection().add(detalleFactura);
                codigoItemNew = em.merge(codigoItemNew);
            }
            em.getTransaction().commit();
        } catch (Exception ex) {
            String msg = ex.getLocalizedMessage();
            if (msg == null || msg.length() == 0) {
                Integer id = detalleFactura.getCodigoDetalleFactura();
                if (findDetalleFactura(id) == null) {
                    throw new NonexistentEntityException("The detalleFactura with id " + id + " no longer exists.");
                }
            }
            throw ex;
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void destroy(Integer id) throws NonexistentEntityException {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            DetalleFactura detalleFactura;
            try {
                detalleFactura = em.getReference(DetalleFactura.class, id);
                detalleFactura.getCodigoDetalleFactura();
            } catch (EntityNotFoundException enfe) {
                throw new NonexistentEntityException("The detalleFactura with id " + id + " no longer exists.", enfe);
            }
            FacturasCabecera codigoFactura = detalleFactura.getCodigoFactura();
            if (codigoFactura != null) {
                codigoFactura.getDetalleFacturaCollection().remove(detalleFactura);
                codigoFactura = em.merge(codigoFactura);
            }
            Item codigoItem = detalleFactura.getCodigoItem();
            if (codigoItem != null) {
                codigoItem.getDetalleFacturaCollection().remove(detalleFactura);
                codigoItem = em.merge(codigoItem);
            }
            em.remove(detalleFactura);
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public List<DetalleFactura> findDetalleFacturaEntities() {
        return findDetalleFacturaEntities(true, -1, -1);
    }

    public List<DetalleFactura> findDetalleFacturaEntities(int maxResults, int firstResult) {
        return findDetalleFacturaEntities(false, maxResults, firstResult);
    }

    private List<DetalleFactura> findDetalleFacturaEntities(boolean all, int maxResults, int firstResult) {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            cq.select(cq.from(DetalleFactura.class));
            Query q = em.createQuery(cq);
            if (!all) {
                q.setMaxResults(maxResults);
                q.setFirstResult(firstResult);
            }
            return q.getResultList();
        } finally {
            em.close();
        }
    }

    public DetalleFactura findDetalleFactura(Integer id) {
        EntityManager em = getEntityManager();
        try {
            return em.find(DetalleFactura.class, id);
        } finally {
            em.close();
        }
    }

    public int getDetalleFacturaCount() {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            Root<DetalleFactura> rt = cq.from(DetalleFactura.class);
            cq.select(em.getCriteriaBuilder().count(rt));
            Query q = em.createQuery(cq);
            return ((Long) q.getSingleResult()).intValue();
        } finally {
            em.close();
        }
    }
    
}
